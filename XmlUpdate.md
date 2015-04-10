# Update a Node or Attribute #
To update a node or attribute, specify the !XPath to identify what to update, and provide the replacement value as inner text.  In the examples below, the string "xmlupdate" will be used as the replacement string.

# Updating an Attribute #
```
<XML source="test.xml">
	<Update xpath="/configuration/nhibernate/add[@key='hibernate.dialect']/@value">xmlupdate</Update>
</XML>
```

# Update a Node #

```
<XML source="test.xml">
	<Update xpath="/configuration/Random">xmlupdate</Update>
</XML>
```