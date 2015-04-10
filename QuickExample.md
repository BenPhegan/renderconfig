## A quick example ##

### The Problem ###
You have an application that runs on:

  1. 8 different servers in production, 2 regions
  1. 4 in test, 2 regions
  1. 2 in dev, 1 region
  1. In total, three environment types, 2 regions, 14 servers. I did mention that this was more for enterprise applications, right?

In each region, there are a set of common configuration (proxy servers, database servers, whatever). There is also a common set of configuration options specific to dev, test and production.

One of the configuration files that you need to update for each instance is provided by a third party, lets say its an NHibernate configuration file. One typical approach to this problem is to hand-craft a set of configuration files for each server (14 in total), and store them in source control. Which is fine, until the underlying NHibernate configuration needs to change, say with the addition of a few completely new settings.

Then your stuck with a lot of manual editing.

### The Solution ###
With RenderConfig, you can still store the settings for each server in source code control, however what you are storing are the transforms required to take a set of source configuration files and output a set of files for each target server. To simplify this process, RenderConfig? allows you to define:

  1. A configuration representing each environment type, for example a set of transforms specific to UAT
  1. A configuration representing each region, for example specific to Sydney
  1. And finally, a configuration representing the specific instance or server, which can depend on as many of the other configuration sets as required (ie server "XYZ" depends on both the "Sydney" and the "UAT" configuration modifications)

As you are storing the modifications required to take an underlying source file (say nhibernate.config) and transform it, the underlying source file can be changed in generic ways that will not cause any additional work to integrate. In the example above, if an additional set of configuration was required, this would be added to the source configuration file...the modifications would then be applied over the top of these modifications. As long as the modifications were not directly impacted by the underlying changes to the configuration file, no further changes would need to be made.

This can be run on both the command line (RenderConfig.exe) and as part of a build process via an MSBuild plugin (MSBuild.RenderConfig.dll).

### Breaking this Down ###
So what would a (partial) solution look like?  Firstly, we need to have a source xml file to look at.

```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="nhibernate" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />
  </configSections>
  <Random>Value</Random>
  <nhibernate>
    <add key="hibernate.connection.provider" value="NHibernate.Connection.DriverConnectionProvider" />
    <add key="hibernate.dialect" value="NHibernate.Dialect.MsSql2000Dialect" />
    <add key="hibernate.connection.driver_class" value="NHibernate.Driver.SqlClientDriver" />
    <add key="hibernate.connection.connection_string" value="Server=localhost;initial catalog=nhibernate;Integrated Security=SSPI" />
  </nhibernate>
</configuration>
```

Now, for this example all we are going to do is set up the following:

  1. Two regional configurations:
    1. A configuration called Sydney
    1. A configuration called London
  1. Two environment configurations
    1. Development
    1. Production
  1. Three server configurations

So, lets have a look at a RenderConfig configuration for this and step through it:

```
<RenderConfig xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="Configuration.xsd">
  <Configurations>
  
    <Configuration Name="sydney">
      <TargetFiles>
        <XML source="config.xml">
          <Update xpath="/configuration/nhibernate/add[@key='hibernate.connection.connection_string']/@value">Server=sydney-DB;initial catalog=nhibernate;Integrated Security=SSPI</Update>
        </XML>
      </TargetFiles>
    </Configuration>
	
    <Configuration Name="london">
      <TargetFiles>
        <XML source="config.xml">
          <Update xpath="/configuration/nhibernate/add[@key='hibernate.connection.connection_string']/@value">Server=london-DB;initialcatalog=nhibernate;Integrated Security=SSPI</Update>
        </XML>
      </TargetFiles>
    </Configuration>
	
    <Configuration Name="development">
      <TargetFiles>
        <XML source="config.xml">
          <Update xpath="/configuration/Random">DEV</Update>
        </XML>
      </TargetFiles>
    </Configuration>
	
    <Configuration Name="uat">
      <TargetFiles>
        <XML source="config.xml">
          <Update xpath="/configuration/Random">UAT</Update>
        </XML>
      </TargetFiles>
    </Configuration>
	
    <Configuration Name="production">
      <TargetFiles>
        <XML source="config.xml">
          <Update xpath="/configuration/Random">PROD</Update>
        </XML>
      </TargetFiles>
    </Configuration>
	
    <Configuration Name="server1" Depends="sydney, development" />
    <Configuration Name="server2" Depends="london, uat" />
    <Configuration Name="server3" Depends="london, uat" />
    <Configuration Name="server4" Depends="sydney, uat" />
    <Configuration Name="server5" Depends="sydney, uat" />
    <Configuration Name="server6" Depends="sydney, development" />
    <Configuration Name="server7" Depends="sydney, production" />
    <Configuration Name="server8" Depends="sydney, production" />
    <Configuration Name="server9" Depends="sydney, production" />
    <Configuration Name="server10" Depends="sydney, production" />
    <Configuration Name="server11" Depends="london, production" />
    <Configuration Name="server12" Depends="london, production" />
    <Configuration Name="server13" Depends="london, production" />
    <Configuration Name="server14" Depends="london, production" />
	
    <Configuration Name="sydney-dev" Depends="sydney, development" />
    <Configuration Name="sydney-uat" Depends="sydney, uat" />
    <Configuration Name="london-uat" Depends="london, uat" />
    <Configuration Name="sydney-prod" Depends="sydney, production" />
    <Configuration Name="london-prod" Depends="london, production" />
  </Configurations>
</RenderConfig>
```

In this instance, we are only dealing with an XML file, however for an INI or text file, the configuration is similar.

What we have defined is a set of `<Configuration>` nodes, with (in this case) a single `<TargetFile>` node specifying our source file, and an `<Update>` to make to the file.  To identify exactly what we want to update, we use an XPath to point out the node or attribute.

To run a configuration, the easiest way would be via the command line.  We are going to get RenderConfig to parse the file provided above, render the "server1" configuration, and output the results to the directory "Server1OutputDirectory".  The results of this are:
```
C:\RenderConfig>RenderConfig.exe -f configurationfile.xml -c server1 -o Server1OutputDirectory
--------------------------------------------------------
                Config File = configurationfile.xml
              Configuration = server1
            Input Directory =
           Output Directory = Server1OutputDirectory
    Delete Output Directory = False
          Break On No Match = True
           Clean XML Output = False
  Preserve Source Structure = False
--------------------------------------------------------
Reading in configuration file...
Found 24 configurations...
Generating Node List...
Generating Node List...
Building dependency queue...
Running modification: development
      SOURCE = config.xml
      TARGET = Server1OutputDirectory\config.xml
                    TYPE = UPDATE
                   XPATH = /configuration/Random
                   VALUE = DEV
                   COUNT = 1

Running modification: sydney
      SOURCE = config.xml
      TARGET = Server1OutputDirectory\config.xml
                    TYPE = UPDATE
                   XPATH = /configuration/nhibernate/add[@key='hibernate.connection.connection_string']/@value
                   VALUE = Server=sydney-DB;initial catalog=nhibernate;Integrated Security=SSPI
                   COUNT = 1

Running modification: server1
Configuration rendered!
```

Now, in this instance the introduction of a `<Configuration>` node for each server is overkill, as we are actually not doing any server specific configuration, so we really only need to provide five different targets as shown at the bottom of the file.