using InstallApp.Logging;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallApp.Synchronization
{
    public class SyncManager
    {
        private string replica1RootPath;
        private string replica2RootPath;
        private SyncOptions syncOption;
        private ICollection<string> fileNameExcludes;
        private ILogger logger;

        public SyncManager(string replica1RootPath, string replica2RootPath, SyncOptions syncOption, ILogger logger)
            : this(replica1RootPath, replica2RootPath, null, syncOption, logger)
        {
        }

        public SyncManager(string replica1RootPath, string replica2RootPath, ICollection<string> fileNameExcludes, SyncOptions syncOption, ILogger logger)
        {
            this.replica1RootPath = replica1RootPath;
            this.replica2RootPath = replica2RootPath;
            this.fileNameExcludes = fileNameExcludes;
            this.syncOption = syncOption;
            this.logger = logger;
        }


        public void Start()
        {
            try
            {
                // Set options for the synchronization session. In this case, options specify
                // that the application will explicitly call FileSyncProvider.DetectChanges, and
                // that items should be moved to the Recycle Bin instead of being permanently deleted.
                FileSyncOptions options =
                    FileSyncOptions.ExplicitDetectChanges |
                    FileSyncOptions.RecycleDeletedFiles |
                    FileSyncOptions.RecyclePreviousFileOnUpdates |
                    FileSyncOptions.RecycleConflictLoserFiles;

                // Create a filter that excludes all *.lnk files. The same filter should be used 
                // by both providers.
                FileSyncScopeFilter filter = new FileSyncScopeFilter();
                if (fileNameExcludes != null && fileNameExcludes.Count > 0)
                {                                       
                    foreach (var item in fileNameExcludes)
	                {
                        filter.FileNameExcludes.Add(item);
	                }                    
                }

                // Explicitly detect changes on both replicas before syncyhronization occurs.
                // This avoids two change detection passes for the bidirectional synchronization 
                // that we will perform.
                DetectChangesOnFileSystemReplica(replica1RootPath, filter, options);
                DetectChangesOnFileSystemReplica(replica2RootPath, filter, options);

                // Synchronize the replicas in both directions. In the first session replica 1 is
                // the source, and in the second session replica 2 is the source. The third parameter
                // (the filter value) is null because the filter is specified in DetectChangesOnFileSystemReplica().
                SyncFileSystemReplicasOneWay(replica1RootPath, replica2RootPath, null, options);
                if (syncOption == SyncOptions.Both)
                {
                    SyncFileSystemReplicasOneWay(replica2RootPath, replica1RootPath, null, options);
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine("\nException from File Sync Provider:\n" + e.ToString());
                if (logger != null)
                {
                    logger.Error(e);
                }
            }
        }

        // Create a provider, and detect changes on the replica that the provider
        // represents.
        private void DetectChangesOnFileSystemReplica(string replicaRootPath, FileSyncScopeFilter filter, FileSyncOptions options)
        {
            FileSyncProvider provider = null;

            try
            {
                provider = new FileSyncProvider(replicaRootPath, filter, options);
                provider.DetectChanges();
            }
            finally
            {
                // Release resources.
                if (provider != null)
                    provider.Dispose();
            }
        }

        private void SyncFileSystemReplicasOneWay(string sourceReplicaRootPath, string destinationReplicaRootPath, FileSyncScopeFilter filter, FileSyncOptions options)
        {
            FileSyncProvider sourceProvider = null;
            FileSyncProvider destinationProvider = null;

            try
            {
                // Instantiate source and destination providers, with a null filter (the filter
                // was specified in DetectChangesOnFileSystemReplica()), and options for both.
                sourceProvider = new FileSyncProvider(sourceReplicaRootPath, filter, options);
                destinationProvider = new FileSyncProvider(destinationReplicaRootPath, filter, options);

                // Register event handlers so that we can write information
                // to the console.
                destinationProvider.AppliedChange += new EventHandler<AppliedChangeEventArgs>(OnAppliedChange);
                destinationProvider.SkippedChange += new EventHandler<SkippedChangeEventArgs>(OnSkippedChange);

                // Use SyncCallbacks for conflicting items.
                SyncCallbacks destinationCallbacks = destinationProvider.DestinationCallbacks;
                destinationCallbacks.ItemConflicting += new EventHandler<ItemConflictingEventArgs>(OnItemConflicting);
                destinationCallbacks.ItemConstraint += new EventHandler<ItemConstraintEventArgs>(OnItemConstraint);

                SyncOrchestrator agent = new SyncOrchestrator();
                agent.LocalProvider = sourceProvider;
                agent.RemoteProvider = destinationProvider;
                agent.Direction = SyncDirectionOrder.Upload; // Upload changes from the source to the destination.

                //Console.WriteLine("Synchronizing changes to replica: " + destinationProvider.RootDirectoryPath);
                if (logger != null)
                {
                    logger.Info("Synchronizing changes to replica: " + destinationProvider.RootDirectoryPath);
                }
                agent.Synchronize();
            }
            finally
            {
                // Release resources.
                if (sourceProvider != null) sourceProvider.Dispose();
                if (destinationProvider != null) destinationProvider.Dispose();
            }
        }

        // Provide information about files that were affected by the synchronization session.
        private void OnAppliedChange(object sender, AppliedChangeEventArgs args)
        {
            switch (args.ChangeType)
            {
                case ChangeType.Create:
                    //Console.WriteLine("-- Applied CREATE for file " + args.NewFilePath);
                    if (logger != null)
                    {
                        logger.Info("-- Applied CREATE for file " + args.NewFilePath);
                    }
                    break;
                case ChangeType.Delete:
                    //Console.WriteLine("-- Applied DELETE for file " + args.OldFilePath);
                    if (logger != null)
                    {
                        logger.Info("-- Applied DELETE for file " + args.OldFilePath);
                    }
                    break;
                case ChangeType.Update:
                    //Console.WriteLine("-- Applied OVERWRITE for file " + args.OldFilePath);
                    if (logger != null)
                    {
                        logger.Info("-- Applied OVERWRITE for file " + args.OldFilePath);
                    }
                    break;
                case ChangeType.Rename:
                    //Console.WriteLine("-- Applied RENAME for file " + args.OldFilePath + " as " + args.NewFilePath);
                    if (logger != null)
                    {
                        logger.Info("-- Applied RENAME for file " + args.OldFilePath + " as " + args.NewFilePath);
                    }
                    break;
            }
        }

        // Provide error information for any changes that were skipped.
        private void OnSkippedChange(object sender, SkippedChangeEventArgs args)
        {
            //Console.WriteLine("-- Skipped applying " + args.ChangeType.ToString().ToUpper()
            //      + " for " + (!string.IsNullOrEmpty(args.CurrentFilePath) ?
            //                    args.CurrentFilePath : args.NewFilePath) + " due to error");

            var message = string.Format("-- Skipped applying {0} for {1} due to error",
                args.ChangeType.ToString().ToUpper(),
                (!string.IsNullOrEmpty(args.CurrentFilePath) ? args.CurrentFilePath : args.NewFilePath));
            if (logger != null)
            {
                logger.Info(message);
            }

            if (args.Exception != null)
            {
                //Console.WriteLine("   [" + args.Exception.Message + "]");
                if (logger != null)
                {
                    logger.Info("   [" + args.Exception.Message + "]");
                }
            }
        }

        // By default, conflicts are resolved in favor of the last writer. In this example,
        // the change from the source in the first session (replica 1), will always
        // win the conflict.
        private void OnItemConflicting(object sender, ItemConflictingEventArgs args)
        {
            args.SetResolutionAction(ConflictResolutionAction.SourceWins);
            //Console.WriteLine("-- Concurrency conflict detected for item " + args.DestinationChange.ItemId.ToString());
            if (logger != null)
            {
                logger.Info("-- Concurrency conflict detected for item " + args.DestinationChange.ItemId.ToString());
            }
        }

        private void OnItemConstraint(object sender, ItemConstraintEventArgs args)
        {
            args.SetResolutionAction(ConstraintConflictResolutionAction.SourceWins);
            //Console.WriteLine("-- Constraint conflict detected for item " + args.DestinationChange.ItemId.ToString());
            if (logger != null)
            {
                logger.Info("-- Constraint conflict detected for item " + args.DestinationChange.ItemId.ToString());
            }
        }
    }

    public enum SyncOptions
    { 
        OneWay,
        Both
    }
}
