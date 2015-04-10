## Using !MSBuild.RenderConfig.dll ##

You can include RenderConfig into your build via an !MSBuild task include, ensuring that wherever and however your software is built, the configuration is built at the same time.

In order to do this, you must:

  1. Create an !MSBuild build script to run the task from.  This will be left as an exercise for the reader, as Google will do a better job of explaining this than I will.
  1. Add a task definition including the !MSBuild.RenderConfig.dll as a valid task in your build script:
```
  <UsingTask TaskName="RenderConfig.MSBuild.RenderConfig" AssemblyFile="MSBuild.RenderConfig.dll"/>
```
  1. Within a defined task, add a call to process a particular configuration (or configurations).  You can use all the same options as you can on the command line, please see the Configuration Settings page for attribute names.
```
  <RenderConfig ConfigFile="test.xml.xml" Configuration="xmlupdate" OutputDirectory=".\testing" />
```