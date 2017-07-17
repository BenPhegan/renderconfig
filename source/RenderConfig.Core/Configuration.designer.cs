﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3603
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=2.0.50727.3038.
// 
using System.Xml.Serialization;
using System.Collections.Generic;
using Core = RenderConfig.Core.Interfaces;

namespace RenderConfig.Core {

    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]

    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="", IsNullable=false)]
    public partial class RenderConfig {
        
        private List<Include> includesField;
        
        private List<Configuration> configurationsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Include", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public List<Include> Includes
        {
            get {
                return this.includesField;
            }
            set {
                this.includesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Configuration", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public List<Configuration> Configurations
        {
            get {
                return this.configurationsField;
            }
            set {
                this.configurationsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]

    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class Include {
        
        private string fileField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string file {
            get {
                return this.fileField;
            }
            set {
                this.fileField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]

    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class IniReplace {
        
        private string regexField;
        
        private string valueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string regex {
            get {
                return this.regexField;
            }
            set {
                this.regexField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]

    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class IniDelete {
        
        private string sectionField;
        
        private string keyField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string section {
            get {
                return this.sectionField;
            }
            set {
                this.sectionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string key {
            get {
                return this.keyField;
            }
            set {
                this.keyField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]

    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class IniUpdate {
        
        private string sectionField;
        
        private string keyField;
        
        private string valueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string section {
            get {
                return this.sectionField;
            }
            set {
                this.sectionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string key {
            get {
                return this.keyField;
            }
            set {
                this.keyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]

    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class IniAdd {
        
        private string sectionField;
        
        private string keyField;
        
        private string valueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string section {
            get {
                return this.sectionField;
            }
            set {
                this.sectionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string key {
            get {
                return this.keyField;
            }
            set {
                this.keyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]

    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class XmlReplace {
        
        private string regexField;
        
        private string valueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string regex {
            get {
                return this.regexField;
            }
            set {
                this.regexField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]

    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class XmlDelete {
        
        private string xpathField;
        
        private string valueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string xpath {
            get {
                return this.xpathField;
            }
            set {
                this.xpathField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]

    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class XmlUpdate {
        
        private string xpathField;
        
        private string valueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string xpath {
            get {
                return this.xpathField;
            }
            set {
                this.xpathField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]

    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class XmlAdd {
        
        private string xpathField;
        
        private string attributeField;
        
        private string valueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string xpath {
            get {
                return this.xpathField;
            }
            set {
                this.xpathField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string attribute {
            get {
                return this.attributeField;
            }
            set {
                this.attributeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]

    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class Configuration {

        private List<EnvironmentVariable> environmentVariablesField;
        
        private TargetFiles targetFilesField;
        
        private string nameField;
        
        private string dependsField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("EnvironmentVariable", typeof(EnvironmentVariable), Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<EnvironmentVariable> EnvironmentVariables {
            get {
                return this.environmentVariablesField;
            }
            set {
                this.environmentVariablesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("TargetFiles", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TargetFiles TargetFiles
        {
            get {
                return this.targetFilesField;
            }
            set {
                this.targetFilesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Depends {
            get {
                return this.dependsField;
            }
            set {
                this.dependsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]

    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class EnvironmentVariable {
        
        private string variableField;
        
        private string valueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string variable {
            get {
                return this.variableField;
            }
            set {
                this.variableField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]

    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class TargetFiles {
        
        private List<XmlTargetFile> xMLField;
        
        private List<IniTargetFile> iNIField;
        
        private List<TxtTargetFile> tXTField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("XML", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<XmlTargetFile> XML {
            get {
                return this.xMLField;
            }
            set {
                this.xMLField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("INI", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<IniTargetFile> INI {
            get {
                return this.iNIField;
            }
            set {
                this.iNIField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("TXT", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<TxtTargetFile> TXT {
            get {
                return this.tXTField;
            }
            set {
                this.tXTField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]

    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class XmlTargetFile : Core.Interfaces.ITargetFile
    {
        
        private List<XmlAdd> addField;
        
        private List<XmlUpdate> updateField;
        
        private List<XmlDelete> deleteField;
        
        private List<XmlReplace> replaceField;
        
        private string sourceField;
        
        private string destinationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Add", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public List<XmlAdd> Add {
            get {
                return this.addField;
            }
            set {
                this.addField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Update", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public List<XmlUpdate> Update {
            get {
                return this.updateField;
            }
            set {
                this.updateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Delete", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public List<XmlDelete> Delete {
            get {
                return this.deleteField;
            }
            set {
                this.deleteField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Replace", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public List<XmlReplace> Replace {
            get {
                return this.replaceField;
            }
            set {
                this.replaceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string source {
            get {
                return this.sourceField;
            }
            set {
                this.sourceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string destination {
            get {
                return this.destinationField;
            }
            set {
                this.destinationField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]

    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class IniTargetFile : Core.Interfaces.ITargetFile {
        
        private List<IniAdd> addField;
        
        private List<IniUpdate> updateField;
        
        private List<IniDelete> deleteField;
        
        private List<IniReplace> replaceField;
        
        private string sourceField;
        
        private string destinationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Add", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public List<IniAdd> Add {
            get {
                return this.addField;
            }
            set {
                this.addField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Update", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public List<IniUpdate> Update {
            get {
                return this.updateField;
            }
            set {
                this.updateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Delete", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public List<IniDelete> Delete {
            get {
                return this.deleteField;
            }
            set {
                this.deleteField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Replace", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public List<IniReplace> Replace {
            get {
                return this.replaceField;
            }
            set {
                this.replaceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string source {
            get {
                return this.sourceField;
            }
            set {
                this.sourceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string destination {
            get {
                return this.destinationField;
            }
            set {
                this.destinationField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]

    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true)]
    public partial class TxtTargetFile : Core.Interfaces.ITargetFile
    {
        
        private IniReplace[] replaceField;
        
        private string sourceField;
        
        private string destinationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Replace", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public IniReplace[] Replace {
            get {
                return this.replaceField;
            }
            set {
                this.replaceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string source {
            get {
                return this.sourceField;
            }
            set {
                this.sourceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string destination {
            get {
                return this.destinationField;
            }
            set {
                this.destinationField = value;
            }
        }
    }
}