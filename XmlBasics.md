# XPath #
Just about all the modifications that you need to make to an XML document are performed based on matching a particular node (or nodes) to an XPath.

Now, there are a lot of different websites and books out there teaching the theory and practice behind XPaths.  You could read all those.  Even if you read just a few, you are going to know more than me.  Or, you can read the snippets below, cut and paste them, and (hopefully) save yourself some time!  This is by no means exhaustive, just a common set I find particularly useful.

For all the examples below, we will use the following XML document.

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

## Select a Node Based on Name ##
Using an XPath of `xpath="/configuration/configSections/section"` will select the node:

```
	<section name="nhibernate" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" />`	
```

## Select a Node Based on Attribute Value ##
Using an XPath of `xpath="/configuration/nhibernate/add[@key='hibernate.dialect']"` will select the node:
```
	<add key="hibernate.dialect" value="NHibernate.Dialect.MsSql2000Dialect" />
```

## Select an Attribute ##
Using an XPath of `xpath="/configuration/nhibernate/add[@key='hibernate.dialect']/@value"` will select the _attribute_ `value="NHibernate.Dialect.MsSql2000Dialect"`, not the parent node.