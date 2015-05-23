# Introduction #

IMDb Mobile v0.6+ will support multi-language interfaces. The data will be stored in an open XML file underneath the "Translations" directory in the install folder.


# Details #

1. Copy Template English.xml

---

Firstly, take a copy of the existing English.xml in the \Program Files\IMDb Mobile\Translations of the device & rename it to your language. The filename will be used to identify the language in the Settings dialog, e.g. Russian.xml.

2. Modify New XML File

---

**NOTE** This XML file must be modified using an editor which will preserve the content encoding. For Windows users, Notepad++ is recommended.

Replace all phrases in the file in between the <![CDATA[...]]> tags with the translated value. The "Phrase" attribute denotes the English value.

**NOTE** Do not change the ID attribute. Any ID attribute which exists in the English.xml MUST exist in the new XML file, otherwise IMDb Mobile may crash

3. Save the XML File to the device

---

Once complete, save the XML file to the device under \Program Files\IMDb Mobile\Translations\ - then restart the application. The option for the new language should be available in the Settings dialog