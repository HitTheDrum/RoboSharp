﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using RoboSharp.Interfaces;
using System.Threading.Tasks;

namespace RoboSharp
{
    /// <summary>
    /// Represents a single RoboCopy Job File
    /// </summary>
    /// <remarks>
    /// For more information, a good resource is here: <see href="https://adamtheautomator.com/robocopy/#Robocopy_Jobs"/>
    /// </remarks>
    public class JobFile : ICloneable, IRoboCommand
    {

        #region < Constructor >

        /// <summary>
        /// Create a JobFile with Default Options
        /// </summary>
        public JobFile() { }

        /// <summary>
        /// Constructor for ICloneable Interface
        /// </summary>
        /// <param name="jobFile"></param>
        public JobFile(JobFile jobFile)
        {
            this.roboCommand = jobFile.roboCommand.Clone();
        }

        /// <summary>
        /// Constructor for Factory Methods
        /// </summary>
        private JobFile(string filePath, RoboCommand cmd)
        {
            FilePath = filePath;
            roboCommand = cmd;
        }

        #endregion

        #region < Factory Methods >

        /// <inheritdoc cref="JobFileBuilder.Parse(string)"/>
        public static JobFile ParseJobFile(string path)
        {
            RoboCommand cmd = JobFileBuilder.Parse(path);
            if (cmd != null) return new JobFile(path, cmd);
            return null;
        }

        /// <inheritdoc cref="JobFileBuilder.Parse(StreamReader)"/>
        public static JobFile ParseJobFile(StreamReader streamReader)
        {
            RoboCommand cmd = JobFileBuilder.Parse(streamReader);
            if (cmd != null) return new JobFile("", cmd);
            return null;
        }

        /// <inheritdoc cref="JobFileBuilder.Parse(FileInfo)"/>
        public static JobFile ParseJobFile(FileInfo file)
        {
            RoboCommand cmd = JobFileBuilder.Parse(file);
            if (cmd != null) return new JobFile(file.FullName, cmd);
            return null;
        }

        /// <inheritdoc cref="JobFileBuilder.Parse(IEnumerable{String})"/>
        public static JobFile ParseJobFile(IEnumerable<string> FileText)
        {
            RoboCommand cmd = JobFileBuilder.Parse(FileText);
            if (cmd != null) return new JobFile("", cmd);
            return null;
        }

        #endregion

        #region < ICLONEABLE >

        /// <summary>
        /// Create a clone of this JobFile
        /// </summary>
        public JobFile Clone() => new JobFile(this);

        object ICloneable.Clone() => Clone();

        #endregion

        #region < Constants >

        /// <summary>
        /// Expected File Extension for Job Files exported from RoboCopy.
        /// </summary>
        public const string JOBFILE_Extension = ".RCJ";

        /// <summary>
        /// FileFilter to use in an to search for this extension, such as with <see cref="DirectoryInfo.GetFiles(string)"/>
        /// </summary>
        public const string JOBFILE_SearchPattern = "*.RCJ";

        /// <summary>
        /// FileFilter to use in a dialog window, such as the OpenFileDialog window.
        /// </summary>

        public const string JOBFILE_DialogFilter = "RoboCopy Job|*.RCJ";
        #endregion


        #region < Fields >

        /// <summary>
        /// Options are stored in a RoboCommand object for simplicity.
        /// </summary>
        private RoboCommand roboCommand;

        #endregion

        #region < Properties >

        /// <summary>FilePath of the Job File </summary>
        public string FilePath { get; set; }

        /// <inheritdoc cref="RoboCommand.Name"/>
        public string Job_Name
        {
            get => roboCommand.Name;
            set => roboCommand.Name = value;
        }

        /// <inheritdoc cref="RoboCommand.LoggingOptions"/>
        public CopyOptions CopyOptions => roboCommand.CopyOptions;

        /// <inheritdoc cref="RoboCommand.LoggingOptions"/>
        public LoggingOptions LoggingOptions => roboCommand.LoggingOptions;

        /// <inheritdoc cref="RoboCommand.LoggingOptions"/>
        public RetryOptions RetryOptions => roboCommand.RetryOptions;

        /// <inheritdoc cref="RoboCommand.LoggingOptions"/>
        public SelectionOptions SelectionOptions => roboCommand.SelectionOptions;

        #endregion

        #region < Methods >


        /// <summary>
        /// Save the JobFile to <paramref name="path"/>
        /// </summary>
        /// <param name="path"></param>
        /// <inheritdoc cref="Save()"/>
        public void Save(string path)
        {
            FilePath = path;
            Save();
        }

        /// <summary>
        /// Save the JobFile to <see cref="FilePath"/>
        /// </summary>
        /// <exception cref="ArgumentException"/>
        public void Save()
        {
            if (FilePath.IsNullOrWhiteSpace()) throw new ArgumentException("FilePath Property is Empty");
        }

        #endregion

        #region < IRoboCommand Interface >

        #region < Events >

        event RoboCommand.FileProcessedHandler IRoboCommand.OnFileProcessed
        {
            add
            {
                ((IRoboCommand)roboCommand).OnFileProcessed += value;
            }

            remove
            {
                ((IRoboCommand)roboCommand).OnFileProcessed -= value;
            }
        }

        event RoboCommand.CommandErrorHandler IRoboCommand.OnCommandError
        {
            add
            {
                ((IRoboCommand)roboCommand).OnCommandError += value;
            }

            remove
            {
                ((IRoboCommand)roboCommand).OnCommandError -= value;
            }
        }

        event RoboCommand.ErrorHandler IRoboCommand.OnError
        {
            add
            {
                ((IRoboCommand)roboCommand).OnError += value;
            }

            remove
            {
                ((IRoboCommand)roboCommand).OnError -= value;
            }
        }

        event RoboCommand.CommandCompletedHandler IRoboCommand.OnCommandCompleted
        {
            add
            {
                ((IRoboCommand)roboCommand).OnCommandCompleted += value;
            }

            remove
            {
                ((IRoboCommand)roboCommand).OnCommandCompleted -= value;
            }
        }

        event RoboCommand.CopyProgressHandler IRoboCommand.OnCopyProgressChanged
        {
            add
            {
                ((IRoboCommand)roboCommand).OnCopyProgressChanged += value;
            }

            remove
            {
                ((IRoboCommand)roboCommand).OnCopyProgressChanged -= value;
            }
        }

        event RoboCommand.ProgressUpdaterCreatedHandler IRoboCommand.OnProgressEstimatorCreated
        {
            add
            {
                ((IRoboCommand)roboCommand).OnProgressEstimatorCreated += value;
            }

            remove
            {
                ((IRoboCommand)roboCommand).OnProgressEstimatorCreated -= value;
            }
        }

        #endregion

        #region < Properties >

        string IRoboCommand.Name => roboCommand.Name;
        bool IRoboCommand.IsPaused => roboCommand.IsPaused;
        bool IRoboCommand.IsRunning => roboCommand.IsRunning;
        bool IRoboCommand.IsScheduled => roboCommand.IsScheduled;
        bool IRoboCommand.IsCancelled => roboCommand.IsCancelled;
        bool IRoboCommand.StopIfDisposing => roboCommand.StopIfDisposing;
        IProgressEstimator IRoboCommand.IProgressEstimator => roboCommand.IProgressEstimator;
        SelectionOptions IRoboCommand.SelectionOptions { get => ((IRoboCommand)roboCommand).SelectionOptions; set => ((IRoboCommand)roboCommand).SelectionOptions = value; }
        RetryOptions IRoboCommand.RetryOptions { get => ((IRoboCommand)roboCommand).RetryOptions; set => ((IRoboCommand)roboCommand).RetryOptions = value; }
        LoggingOptions IRoboCommand.LoggingOptions { get => roboCommand.LoggingOptions; set => roboCommand.LoggingOptions = value; }
        CopyOptions IRoboCommand.CopyOptions { get => ((IRoboCommand)roboCommand).CopyOptions; set => ((IRoboCommand)roboCommand).CopyOptions = value; }
        JobOptions IRoboCommand.JobOptions { get => ((IRoboCommand)roboCommand).JobOptions; }
        RoboSharpConfiguration IRoboCommand.Configuration => roboCommand.Configuration; 
        string IRoboCommand.CommandOptions => roboCommand.CommandOptions;

        #endregion

        #region < Methods >

        void IRoboCommand.Pause()
        {
            ((IRoboCommand)roboCommand).Pause();
        }

        void IRoboCommand.Resume()
        {
            ((IRoboCommand)roboCommand).Resume();
        }

        Task IRoboCommand.Start(string domain, string username, string password)
        {
            return ((IRoboCommand)roboCommand).Start(domain, username, password);
        }

        void IRoboCommand.Stop()
        {
            ((IRoboCommand)roboCommand).Stop();
        }

        void IRoboCommand.Dispose()
        {
            roboCommand.Stop();
        }
        #endregion

        #endregion
    }
}