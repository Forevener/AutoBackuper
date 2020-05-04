# AutoBackuper
Application for automatic archiving changed files or directories

Allows user to set up a watched folders, where program will be monitoring the changes for all elements (files and directories) and archiving the changed ones.

Notes:
Program will not check for changes between the running sessions. Please create the issue if it is really an issue for you.
Program will not backup the changed file if time interval between changes is less than specified in settings - even after the interval passes. That is intended - just keep the interval at 0 if you need all the backups.
