# Add an Attribute #
Adding an attribute is a little different to all the other operations on an XML file, as you will need to provide an additional _attribute_ attribute to identify the attribute name.

```
<XML source="test.xml">
  <Add attribute="value" xpath="/configuration/configSections/section">xmladd</Add>
</XML>
```

# Add a Fragment #
When you need to add a chunk of XML at a particular point in a document, you can identify the parent node via XPath and then simply wrap the XML snippet in a `![CDATA[]]` wrapper.

```
<XML source="test.xml">
  <Add xpath="/configuration/nhibernate"><![CDATA[<Node><Test>xmladd</Test></Node>]]></Add>
</XML>
```

# Add a Node #
To add a node, simply provide the XPath to the parent node, and provide a value.

```
<XML source="test.xml">
  <Add xpath="/configuration/nhibernate">SimpleNode</Add>
</XML>
```