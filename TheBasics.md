## The Basics ##

The general workflow followed requires:
  1. A Configuration file with one or more `<Configuration>` nodes
  1. A set of source files
  1. A RenderConfig engine, either MSBuild.RenderConfig.dll run via MSBuild, or RenderConfig.exe run via the command line.
  1. An output directory, which doesnt need to exist as it will be created when the application is run

When a configuration file is passed to a render engine with both a target Configuration to run and an output directory, the source files will be transformed by the modifications listed by the target configuration, and any other configurations that the target depends on.

## Configurations and Dependencies ##

Each `<Configuration>` node can state one or more dependencies, which are listed as a comma seperated list in the `dependencies=''` attribute.  For example:
```
    <Configuration Name="Primary" Depends="Dependency1, Dependency2">
```

These configurations are used to build up a directed acyclic graph (hopefully!) using a depth first algorithm.  These configurations are run from the leaf nodes back up to the primary target configuration.

## Environment Variables ##

Environment variables can be used to provide further flexibility in building configuration modifications.  The basic mechanism used to include an environment as shown below:
```
    <Configuration Name="variables">
      <TargetFiles>
        <XML source="test.xml">
          <Update xpath="/configuration/Random">$(VariableSetting)</Update>
        </XML>
      </TargetFiles>
    </Configuration>
```

In this example the token "$(VariableSetting)" would be replaced with the value of the environment variable "VariableSetting".

You can also declare environment variables as part of you `<Configuration>` node.  This allows dependencies to be set up between configurations, and have the target configuration override via environment variables.

For example, the set of configurations below allows a set of modifications to be defined, and then a set of target dependencies to be defined that set environment variables to be used in those configurations.

```
    <Configuration Name="variables">
      <!-- The environment variable below will only get used if Variables is not used as a dependency which also defines this variable-->
      <EnvironmentVariables>
        <EnvironmentVariable variable="VariableSetting">variables</EnvironmentVariable>
      </EnvironmentVariables>
      <TargetFiles>
        <XML source="test.xml">
          <Update xpath="/configuration/Random">$(VariableSetting)</Update>
        </XML>
      </TargetFiles>
    </Configuration>

    <Configuration Name="variabletest1" Depends="variables">
      <EnvironmentVariables>
        <EnvironmentVariable variable="VariableSetting">variabletest1</EnvironmentVariable>
      </EnvironmentVariables>
    </Configuration>

    <Configuration Name="variabletest2" Depends="variables">
      <EnvironmentVariables>
        <EnvironmentVariable variable="VariableSetting">variabletest2</EnvironmentVariable>
      </EnvironmentVariables>
    </Configuration>
```

Beware of environment variable set order:
  1. Always leaf to trunk
  1. Prior to any modifications being run

This results in the following behaviour:
  1. If the "variabletest1" configuration was the target, the "variables" target would have its environment variables set first, setting "VariableSetting"="variables"
  1. The environment variables defined in "variabletest1" are the processed, resulting in the environment variable being overwritten, resulting in  "VariableSetting"="variabletest1"
  1. The modifications are then run from the "variables" configuration
  1. Any modifications in "variabletest1" will then be run in (in this case none).