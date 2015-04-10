# Delete a Node #
To delete a node, provide an XPath, for example the two below both delete a node, one selects via an attribute value, the other just by the node name.

```
<XML source="test.xml">
  <Delete xpath="/configuration/nhibernate/add[@key='hibernate.dialect']"/>
</XML>
```

```
<XML source="test.xml">
  <Delete xpath="/configuration/configSections/section"/>
</XML>
```

# Delete an Attribute #
To delete an attribute, again all that is required is an XPath specifying a particular Attribute.  The snippet below will delete the attribute named "value" from the selected node.

```
<XML source="test.xml">
  <Delete xpath="/configuration/nhibernate/add[@key='hibernate.connection.driver_class']/@value"/>
</XML>
```
