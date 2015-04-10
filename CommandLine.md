## Using RenderConfig.exe ##

One of the easiest ways to get started using RenderConfig is to use the command line version, aptly named RenderConfig.exe.

If you are using the default distribution of RenderConfig, you will have an Examples directory in the root directory of your install location, at the same level as the RenderConfig.exe file.  As a quick test to make sure everything is working, you can navigate to this directory from a command prompt and type the following:

```
	RenderConfig.exe -f Examples\config.xml.xml -i Examples -o SanityTest -c xmlupdate
```

To explain what this is asking RenderConfig to do:

  * -f = Specifies an input file from which to read the configurations, one of which will be the configuration that we want to render.
  * -i = Specifies a base input directory.  All target files that are referenced by the configuration file are based on this directory if this is provided.  Otherwise, they are based on the current directory.
  * -o = Specifies the output directory.  This will create a directory called SanityTest under the current directory, and place any modified output files under here.
  * -c = Finally, the configuration target that we would like to run.

You could alternatively provide a -s on the command line and omit an output directory.  A -s will result in a subdirectory being created for each configuration target specified by -c.  All valid configuration settings can be found under Settings.

This should result in the following output:

```
--------------------------------------------------------
                Config File = Examples/config.xml.xml
              Configuration = xmlupdate
            Input Directory = Examples
           Output Directory = SanityTest
    Delete Output Directory = False
          Break On No Match = True
           Clean XML Output = False
  Preserve Source Structure = False
--------------------------------------------------------
Reading in configuration file...
Attempting to include file: config.entity.configuration.xml
Found 13 configurations...
Generating Node List...
Generating Node List...
Creating output directory...
Building dependency queue...
Running modification: xmlupdate
      SOURCE = Examples/test.xml
      TARGET = SanityTest/xmlupdate.xml
                    TYPE = UPDATE
                   XPATH = /configuration/nhibernate/add[@key='hibernate.dialect']/@value
                   VALUE = xmlupdate
                   COUNT = 1

                    TYPE = UPDATE
                   XPATH = /configuration/Random
                   VALUE = xmlupdate
                   COUNT = 1

Configuration rendered!

```