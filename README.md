## About ##
RenderConfig allows the definition of a set of modifications that can be used to transform (or _render_) a set of source files to provide tailored configuration specific to a particular instance of an application.

A typical use case for this is where your application requires a set of configuration files that identify multiple servers that provide particular application services. These application servers will differ depending on whether you are running the application in development, testing or in production.

With RenderConfig, at build time you can explicitly produce a set of configuration files that support all of these configurations, providing the ability to ship the configuration with the other build artefacts.

## Features ##
  1. Takes a target file and copies to an output location, with the same or different filename, whilst applying a set of modifications.
  1. Allows creation of dependencies within configuration sets, ie "Development" configuration requires "Sydney" specific configuration which requires "Common".
  1. Provided as:
      1. A single assembly MSBuild plugin (MSBuild.RenderConfig.dll)
      1. A command line executable (RenderConfig.exe)
  1. Extensible (in code currently) to support many more types of configuration. Stay tuned, the list will grow.
  1. Manipulation of XML files
      1. Create/Update/Delete nodes and attributes based on XPath identification of change
      1. Works despite namespaces (ignores them...I know I know, but I just don't like them!)
      1. Keyword replacement
  1. Manipulation of INI files
      1. Create/Update/Delete keys, values and sections
      1. Keyword replacement
  1. Standard text files
      1. Keyword replacement
  1. Supports splitting of configurations into multiple files via _includes_
  1. Allows use of environment variables in modifications
      1. Environment variables can also be defined within a configuration

For a quick example, see [here](https://github.com/BenPhegan/renderconfig/wiki/QuickExample).
