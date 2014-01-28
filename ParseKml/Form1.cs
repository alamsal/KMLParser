using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Collections.Specialized;
using BK.Util;


namespace ParseKml
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Dictionary<string, List<string>> dictionary =   new Dictionary<string, List<string>>();
        private void button2_Click(object sender, EventArgs e)
        {
            Stream outputfile;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((outputfile = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    txtbxOutput.Text = saveFileDialog1.FileName;
                    outputfile.Close();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }


        private string getRandColor(int count)
        {
            string color;
            string[] kmlColors = { "fffb9a99","ffe31a1c","fffdbf6f","ffff7f00","ffa6cee3","ff1f78b4","ffb2df8a","ff33a02c","ffcab2d6","ff6a3d9a","ffffff99","ffb15928","ff140000","ff14F000","ff780000","ff780078","ff7800F0","ff007800","ff0078F0","ffF00014","ff1400FF","ff14F0FF","ff78FF00","ff7882F0","ffFF7800","ffFF78F0","ff00FF14","ffF0FF14","ff006E14","ff143C8C" };
            int index=(int)(count%kmlColors.Length);
            color = kmlColors[index];
            return color;
        }

        private void Input_Click(object sender, EventArgs e)
        {
            Stream inputfile = null;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((inputfile = openFileDialog1.OpenFile()) != null)
                    {
                        txtbxInput.Text = openFileDialog1.FileName;

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void Convert_Click(object sender, EventArgs e)
        {
            label1.Text = "Please wait working.....";
            //Grouping multiple placemarks based upon same location name
            NameValueCollection keyPlacemarks = new NameValueCollection();
            Dictionary<string, string>mapStyleColor = new Dictionary<string, string>();


            string xmlHead = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                            <kml xmlns=""http://www.opengis.net/kml/2.2"" xmlns:gx=""http://www.google.com/kml/ext/2.2"" xmlns:kml=""http://www.opengis.net/kml/2.2"" xmlns:atom=""http://www.w3.org/2005/Atom"">
                            <Document>
	                            <name>Rearranged placemarks-  KML</name>
	                            <Schema name=""2013 Locations - Snow Only 122913"" id=""S_2013_Locations___Snow_Only_122913_SSSISSSSSSSSSIDD"">
		                            <SimpleField type=""string"" name=""LOCATION_NAME""><displayName>&lt;b&gt;LOCATION NAME&lt;/b&gt;</displayName>
                            </SimpleField>
		                            <SimpleField type=""string"" name=""CONTRACTOR_NAME""><displayName>&lt;b&gt;CONTRACTOR NAME&lt;/b&gt;</displayName>
                            </SimpleField>
		                            <SimpleField type=""string"" name=""CONTRACTOR_CONTACT""><displayName>&lt;b&gt;CONTRACTOR CONTACT&lt;/b&gt;</displayName>
                            </SimpleField>
		                            <SimpleField type=""int"" name=""CONTRACTOR_ID""><displayName>&lt;b&gt;CONTRACTOR ID&lt;/b&gt;</displayName>
                            </SimpleField>
		                            <SimpleField type=""string"" name=""CONTRACTOR_24HR""><displayName>&lt;b&gt;CONTRACTOR 24HR&lt;/b&gt;</displayName>
                            </SimpleField>
		                            <SimpleField type=""string"" name=""CONTRACTOR_OFFICE""><displayName>&lt;b&gt;CONTRACTOR OFFICE&lt;/b&gt;</displayName>
                            </SimpleField>
		                            <SimpleField type=""string"" name=""CONTRACTOR_ALT_24HR""><displayName>&lt;b&gt;CONTRACTOR ALT 24HR&lt;/b&gt;</displayName>
                            </SimpleField>
		                            <SimpleField type=""string"" name=""CUSTOMER""><displayName>&lt;b&gt;CUSTOMER&lt;/b&gt;</displayName>
                            </SimpleField>
		                            <SimpleField type=""string"" name=""CUSTOMER_CODE""><displayName>&lt;b&gt;CUSTOMER CODE&lt;/b&gt;</displayName>
                            </SimpleField>
		                            <SimpleField type=""string"" name=""CUSTOMER_PHONE""><displayName>&lt;b&gt;CUSTOMER PHONE&lt;/b&gt;</displayName>
                            </SimpleField>
		                            <SimpleField type=""string"" name=""CUSTOMER_ADDRESS""><displayName>&lt;b&gt;CUSTOMER ADDRESS&lt;/b&gt;</displayName>
                            </SimpleField>
		                            <SimpleField type=""string"" name=""CUSTOMER_CITY""><displayName>&lt;b&gt;CUSTOMER CITY&lt;/b&gt;</displayName>
                            </SimpleField>
		                            <SimpleField type=""string"" name=""CUSTOMER_STATE""><displayName>&lt;b&gt;CUSTOMER STATE&lt;/b&gt;</displayName>
                            </SimpleField>
		                            <SimpleField type=""int"" name=""CUSTOMER_ZIP_CODE""><displayName>&lt;b&gt;CUSTOMER ZIP CODE&lt;/b&gt;</displayName>
                            </SimpleField>
		                            <SimpleField type=""double"" name=""LONGITUDE""><displayName>&lt;b&gt;LONGITUDE&lt;/b&gt;</displayName>
                            </SimpleField>
		                    <SimpleField type=""double"" name=""LATITUDE""><displayName>&lt;b&gt;LATITUDE&lt;/b&gt;</displayName>
                    </SimpleField>
	                    </Schema>";




            string readInputfile =txtbxInput.Text.ToString();
            //var xdoc = XDocument.Load("C:/Users/Ashis/Dropbox/FreeLance/Cherry/ParseKml/ParseKml/bin/Debug/doc.kml");
            var xdoc = XDocument.Load(readInputfile);

            var ns = new XmlNamespaceManager(new NameTable());
            ns.AddNamespace("kml", "http://www.opengis.net/kml/2.2");


            XNamespace ns1 = "http://www.opengis.net/kml/2.2";
            var placemarks = xdoc
                        .Descendants(ns1 + "Placemark")
                        .Select(p => new
                        {
                          //  Name = p.Element(ns1 + "name").Value,
                            LOCATION_NAME = p.Elements(ns1 + "ExtendedData").Elements(ns1 + "SchemaData").Descendants(ns1 + "SimpleData").Single(x => (string)x.Attribute("name") == "LOCATION_NAME").Value,
                            CONTRACTOR_NAME = p.Elements(ns1 + "ExtendedData").Elements(ns1 + "SchemaData").Descendants(ns1 + "SimpleData").Single(x => (string)x.Attribute("name") == "CONTRACTOR_NAME").Value,
                            CONTRACTOR_CONTACT = p.Elements(ns1 + "ExtendedData").Elements(ns1 + "SchemaData").Descendants(ns1 + "SimpleData").Single(x => (string)x.Attribute("name") == "CONTRACTOR_CONTACT").Value,
                            CONTRACTOR_ID = p.Elements(ns1 + "ExtendedData").Elements(ns1 + "SchemaData").Descendants(ns1 + "SimpleData").Single(x => (string)x.Attribute("name") == "CONTRACTOR_ID").Value,
                            CONTRACTOR_24HR = p.Elements(ns1 + "ExtendedData").Elements(ns1 + "SchemaData").Descendants(ns1 + "SimpleData").Single(x => (string)x.Attribute("name") == "CONTRACTOR_24HR").Value,
                            CONTRACTOR_OFFICE = p.Elements(ns1 + "ExtendedData").Elements(ns1 + "SchemaData").Descendants(ns1 + "SimpleData").Single(x => (string)x.Attribute("name") == "CONTRACTOR_OFFICE").Value,
                            CONTRACTOR_ALT_24HR = p.Elements(ns1 + "ExtendedData").Elements(ns1 + "SchemaData").Descendants(ns1 + "SimpleData").Single(x => (string)x.Attribute("name") == "CONTRACTOR_ALT_24HR").Value,
                            CUSTOMER = p.Elements(ns1 + "ExtendedData").Elements(ns1 + "SchemaData").Descendants(ns1 + "SimpleData").Single(x => (string)x.Attribute("name") == "CUSTOMER").Value,
                            CUSTOMER_CODE = p.Elements(ns1 + "ExtendedData").Elements(ns1 + "SchemaData").Descendants(ns1 + "SimpleData").Single(x => (string)x.Attribute("name") == "CUSTOMER_CODE").Value,
                            CUSTOMER_PHONE = p.Elements(ns1 + "ExtendedData").Elements(ns1 + "SchemaData").Descendants(ns1 + "SimpleData").Single(x => (string)x.Attribute("name") == "CUSTOMER_PHONE").Value,
                            CUSTOMER_ADDRESS = p.Elements(ns1 + "ExtendedData").Elements(ns1 + "SchemaData").Descendants(ns1 + "SimpleData").Single(x => (string)x.Attribute("name") == "CUSTOMER_ADDRESS").Value,
                            CUSTOMER_CITY = p.Elements(ns1 + "ExtendedData").Elements(ns1 + "SchemaData").Descendants(ns1 + "SimpleData").Single(x => (string)x.Attribute("name") == "CUSTOMER_CITY").Value,
                            CUSTOMER_STATE = p.Elements(ns1 + "ExtendedData").Elements(ns1 + "SchemaData").Descendants(ns1 + "SimpleData").Single(x => (string)x.Attribute("name") == "CUSTOMER_STATE").Value,
                            CUSTOMER_ZIP_CODE = p.Elements(ns1 + "ExtendedData").Elements(ns1 + "SchemaData").Descendants(ns1 + "SimpleData").Single(x => (string)x.Attribute("name") == "CUSTOMER_ZIP_CODE").Value,
                            LONGITUDE = p.Elements(ns1 + "ExtendedData").Elements(ns1 + "SchemaData").Descendants(ns1 + "SimpleData").Single(x => (string)x.Attribute("name") == "LONGITUDE").Value,
                            LATITUDE = p.Elements(ns1 + "ExtendedData").Elements(ns1 + "SchemaData").Descendants(ns1 + "SimpleData").Single(x => (string)x.Attribute("name") == "LATITUDE").Value,
                            
                        })
                        .ToList();


            //Exclude duplicates.
            var contractorCount = placemarks.Select(x => x.CONTRACTOR_NAME).Distinct().ToList();


            //Implementing styles
            int count = 0; //total number of contractors
             
            string hcolor,pcolor,finalStyle="";
            //Generate styles
            for (int contrator = 0; contrator < contractorCount.Count;contrator++ )
            {
                string hlightptStyle = @"	<Style id=""hlightPointStyle"">
		                            <IconStyle>
			                            <color>ff7f5555</color>
			                            <Icon>
				                            <href>http://maps.google.com/mapfiles/kml/shapes/placemark_circle_highlight.png</href>
			                            </Icon>
		                            </IconStyle>
		                            <LabelStyle>
			                            <color>ff7f5555</color>
			                            <scale>0.6</scale>
		                            </LabelStyle>
		                            <BalloonStyle>
			                            <text><![CDATA[<table border=""0"">
                              <tr><td><b>LOCATION NAME</b></td><td>$[2013 Locations - Snow Only 122913/LOCATION_NAME]</td></tr>
                              <tr><td><b>CONTRACTOR NAME</b></td><td>$[2013 Locations - Snow Only 122913/CONTRACTOR_NAME]</td></tr>
                              <tr><td><b>CONTRACTOR CONTACT</b></td><td>$[2013 Locations - Snow Only 122913/CONTRACTOR_CONTACT]</td></tr>
                              <tr><td><b>CONTRACTOR ID</b></td><td>$[2013 Locations - Snow Only 122913/CONTRACTOR_ID]</td></tr>
                              <tr><td><b>CONTRACTOR 24HR</b></td><td>$[2013 Locations - Snow Only 122913/CONTRACTOR_24HR]</td></tr>
                              <tr><td><b>CONTRACTOR OFFICE</b></td><td>$[2013 Locations - Snow Only 122913/CONTRACTOR_OFFICE]</td></tr>
                              <tr><td><b>CONTRACTOR ALT 24HR</b></td><td>$[2013 Locations - Snow Only 122913/CONTRACTOR_ALT_24HR]</td></tr>
                              <tr><td><b>CUSTOMER</b></td><td>$[2013 Locations - Snow Only 122913/CUSTOMER]</td></tr>
                              <tr><td><b>CUSTOMER CODE</b></td><td>$[2013 Locations - Snow Only 122913/CUSTOMER_CODE]</td></tr>
                              <tr><td><b>CUSTOMER PHONE</b></td><td>$[2013 Locations - Snow Only 122913/CUSTOMER_PHONE]</td></tr>
                              <tr><td><b>CUSTOMER ADDRESS</b></td><td>$[2013 Locations - Snow Only 122913/CUSTOMER_ADDRESS]</td></tr>
                              <tr><td><b>CUSTOMER CITY</b></td><td>$[2013 Locations - Snow Only 122913/CUSTOMER_CITY]</td></tr>
                              <tr><td><b>CUSTOMER STATE</b></td><td>$[2013 Locations - Snow Only 122913/CUSTOMER_STATE]</td></tr>
                              <tr><td><b>CUSTOMER ZIP CODE</b></td><td>$[2013 Locations - Snow Only 122913/CUSTOMER_ZIP_CODE]</td></tr>
                              <tr><td><b>LONGITUDE</b></td><td>$[2013 Locations - Snow Only 122913/LONGITUDE]</td></tr>
                              <tr><td><b>LATITUDE</b></td><td>$[2013 Locations - Snow Only 122913/LATITUDE]</td></tr>
                            </table>]]></text>
		                            </BalloonStyle>
	                            </Style>";

                //Norm point style
                string normptStyle = @"	<Style id=""normPointStyle"">
		                            <IconStyle>
			                            <color>ff00aaaa</color>
			                            <Icon>
				                            <href>http://maps.google.com/mapfiles/kml/shapes/placemark_circle.png</href>
			                            </Icon>
		                            </IconStyle>
		                            <LabelStyle>
			                            <color>ff00aaaa</color>
			                            <scale>0.6</scale>
		                            </LabelStyle>
		                            <BalloonStyle>
			                            <text><![CDATA[<table border=""0"">
                              <tr><td><b>LOCATION NAME</b></td><td>$[2013 Locations - Snow Only 122913/LOCATION_NAME]</td></tr>
                              <tr><td><b>CONTRACTOR NAME</b></td><td>$[2013 Locations - Snow Only 122913/CONTRACTOR_NAME]</td></tr>
                              <tr><td><b>CONTRACTOR CONTACT</b></td><td>$[2013 Locations - Snow Only 122913/CONTRACTOR_CONTACT]</td></tr>
                              <tr><td><b>CONTRACTOR ID</b></td><td>$[2013 Locations - Snow Only 122913/CONTRACTOR_ID]</td></tr>
                              <tr><td><b>CONTRACTOR 24HR</b></td><td>$[2013 Locations - Snow Only 122913/CONTRACTOR_24HR]</td></tr>
                              <tr><td><b>CONTRACTOR OFFICE</b></td><td>$[2013 Locations - Snow Only 122913/CONTRACTOR_OFFICE]</td></tr>
                              <tr><td><b>CONTRACTOR ALT 24HR</b></td><td>$[2013 Locations - Snow Only 122913/CONTRACTOR_ALT_24HR]</td></tr>
                              <tr><td><b>CUSTOMER</b></td><td>$[2013 Locations - Snow Only 122913/CUSTOMER]</td></tr>
                              <tr><td><b>CUSTOMER CODE</b></td><td>$[2013 Locations - Snow Only 122913/CUSTOMER_CODE]</td></tr>
                              <tr><td><b>CUSTOMER PHONE</b></td><td>$[2013 Locations - Snow Only 122913/CUSTOMER_PHONE]</td></tr>
                              <tr><td><b>CUSTOMER ADDRESS</b></td><td>$[2013 Locations - Snow Only 122913/CUSTOMER_ADDRESS]</td></tr>
                              <tr><td><b>CUSTOMER CITY</b></td><td>$[2013 Locations - Snow Only 122913/CUSTOMER_CITY]</td></tr>
                              <tr><td><b>CUSTOMER STATE</b></td><td>$[2013 Locations - Snow Only 122913/CUSTOMER_STATE]</td></tr>
                              <tr><td><b>CUSTOMER ZIP CODE</b></td><td>$[2013 Locations - Snow Only 122913/CUSTOMER_ZIP_CODE]</td></tr>
                              <tr><td><b>LONGITUDE</b></td><td>$[2013 Locations - Snow Only 122913/LONGITUDE]</td></tr>
                              <tr><td><b>LATITUDE</b></td><td>$[2013 Locations - Snow Only 122913/LATITUDE]</td></tr>
                            </table>]]></text>
		                            </BalloonStyle>
	                            </Style>";


                //Point style map
                string pointstMap = @"	<StyleMap id=""pointStyleMap"">
		                        <Pair>
			                        <key>normal</key>
			                        <styleUrl>#normPointStyle1</styleUrl>
		                        </Pair>
		                        <Pair>
			                        <key>highlight</key>
			                        <styleUrl>#hlightPointStyle1</styleUrl>
		                        </Pair>
	                        </StyleMap>";

                hcolor = getRandColor(count);//Get random color
                //Edit the hstyles 
                StringBuilder sb0 = new StringBuilder(hlightptStyle);
                sb0.Replace("hlightPointStyle", "hlightPointStyle" + count.ToString());
                sb0.Replace("ff7f5555", hcolor);
                hlightptStyle = sb0.ToString();

                pcolor = getRandColor(count+1);//Get random color

                //Edit the ptstyles 
                StringBuilder sb1 = new StringBuilder(normptStyle);
                sb1.Replace("normPointStyle", "normPointStyle" + count.ToString());
                sb1.Replace("ff00aaaa", pcolor);
                normptStyle = sb1.ToString();

                //Edit the styles url
                StringBuilder sb2 = new StringBuilder(pointstMap);
                sb2.Replace("pointStyleMap", "pointStyleMap" + count.ToString());
                sb2.Replace("#normPointStyle1", "#normPointStyle" + count.ToString());
                sb2.Replace("#hlightPointStyle1", "#hlightPointStyle" + count.ToString());
                pointstMap = sb2.ToString();

                //Final style
                finalStyle = finalStyle + hlightptStyle + normptStyle + pointstMap;

                mapStyleColor.Add(contractorCount[count], "#pointStyleMap"+count);
                count++;
            }

            //End of kml
            string xmlTail = @"</Document> </kml>";


            //Rearrange KML Placemarks
            string rearrangedPlacemarks=null;

            foreach (var pmarks in placemarks)
            {
                using(StringWriter str = new StringWriter())
                using (XmlTextWriter xml = new XmlTextWriter(str))
                {
                    xml.Formatting = Formatting.Indented;
                    string templacemarks = null;
                    // Root.
                    xml.WriteStartElement("Placemark");//Start placemark

                    xml.WriteStartElement("name");
                    xml.WriteString(pmarks.LOCATION_NAME);
                    xml.WriteEndElement();

                    xml.WriteStartElement("styleUrl");
                    
                    xml.WriteString(mapStyleColor[pmarks.CONTRACTOR_NAME].ToString());
                    //xml.WriteString("#" + "pointStyleMap0");
                    xml.WriteEndElement();


                    xml.WriteStartElement("ExtendedData"); //start extended data
                    xml.WriteStartElement("SchemaData");   //start schema data
                    xml.WriteAttributeString("schemaUrl", "#S_2013_Locations___Snow_Only_122913_SSSISSSSSSSSSIDD");

                    xml.WriteStartElement("SimpleData");
                    xml.WriteAttributeString("name", "LOCATION_NAME");
                    xml.WriteString(pmarks.LOCATION_NAME);
                    xml.WriteEndElement();

                    xml.WriteStartElement("SimpleData");
                    xml.WriteAttributeString("name", "CONTRACTOR_NAME");
                    xml.WriteString(pmarks.CONTRACTOR_NAME);
                    xml.WriteEndElement();

                    xml.WriteStartElement("SimpleData");
                    xml.WriteAttributeString("name", "CONTRACTOR_CONTACT");
                    xml.WriteString(pmarks.CONTRACTOR_CONTACT);
                    xml.WriteEndElement();

                    xml.WriteStartElement("SimpleData");
                    xml.WriteAttributeString("name", "CONTRACTOR_ID");
                    xml.WriteString(pmarks.CONTRACTOR_ID);
                    xml.WriteEndElement();

                    xml.WriteStartElement("SimpleData");
                    xml.WriteAttributeString("name", "CONTRACTOR_24HR");
                    xml.WriteString(pmarks.CONTRACTOR_24HR);
                    xml.WriteEndElement();

                    xml.WriteStartElement("SimpleData");
                    xml.WriteAttributeString("name", "CONTRACTOR_OFFICE");
                    xml.WriteString(pmarks.CONTRACTOR_OFFICE);
                    xml.WriteEndElement();

                    xml.WriteStartElement("SimpleData");
                    xml.WriteAttributeString("name", "CONTRACTOR_ALT_24HR");
                    xml.WriteString(pmarks.CONTRACTOR_ALT_24HR);
                    xml.WriteEndElement();

                    xml.WriteStartElement("SimpleData");
                    xml.WriteAttributeString("name", "CUSTOMER");
                    xml.WriteString(pmarks.CUSTOMER);
                    xml.WriteEndElement();

                    xml.WriteStartElement("SimpleData");
                    xml.WriteAttributeString("name", "CUSTOMER_CODE");
                    xml.WriteString(pmarks.CUSTOMER_CODE);
                    xml.WriteEndElement();

                    xml.WriteStartElement("SimpleData");
                    xml.WriteAttributeString("name", "CUSTOMER_PHONE");
                    xml.WriteString(pmarks.CUSTOMER_PHONE);
                    xml.WriteEndElement();

                    xml.WriteStartElement("SimpleData");
                    xml.WriteAttributeString("name", "CUSTOMER_ADDRESS");
                    xml.WriteString(pmarks.CUSTOMER_ADDRESS);
                    xml.WriteEndElement();

                    xml.WriteStartElement("SimpleData");
                    xml.WriteAttributeString("name", "CUSTOMER_CITY");
                    xml.WriteString(pmarks.CUSTOMER_CITY);
                    xml.WriteEndElement();

                    xml.WriteStartElement("SimpleData");
                    xml.WriteAttributeString("name", "CUSTOMER_STATE");
                    xml.WriteString(pmarks.CUSTOMER_STATE);
                    xml.WriteEndElement();

                    xml.WriteStartElement("SimpleData");
                    xml.WriteAttributeString("name", "CUSTOMER_ZIP_CODE");
                    xml.WriteString(pmarks.CUSTOMER_ZIP_CODE);
                    xml.WriteEndElement();

                    xml.WriteStartElement("SimpleData");
                    xml.WriteAttributeString("name", "LONGITUDE");
                    xml.WriteString(pmarks.LONGITUDE);
                    xml.WriteEndElement();

                    xml.WriteStartElement("SimpleData");
                    xml.WriteAttributeString("name", "LATITUDE");
                    xml.WriteString(pmarks.LATITUDE);
                    xml.WriteEndElement();



                    xml.WriteEndElement();//end schema data
                    xml.WriteEndElement();//end extended data

                    xml.WriteStartElement("Point"); //start point
                    xml.WriteStartElement("coordinates"); //start point
                    xml.WriteString(pmarks.LONGITUDE + "," + pmarks.LATITUDE + ",0");
                    xml.WriteEndElement(); // end <coordinates>
                    xml.WriteEndElement(); // end <Point>

                    xml.WriteEndElement();//end placemark data

                    // Result is a string
                    templacemarks = str.ToString();
                    rearrangedPlacemarks = rearrangedPlacemarks + templacemarks;
                    //Console.WriteLine("Length: {0}", result.Length);
                    //Console.WriteLine("Result: {0}", result);
                    keyPlacemarks.Add(pmarks.CONTRACTOR_NAME, str.ToString());
                }
            }
            
            //Placemarks rearrangement
            string rearrangedFolder = null;
            for (int index = 0; index < keyPlacemarks.Count; index++)
            {
                string tempFolder = null;

                Console.WriteLine("<Folder>" + keyPlacemarks[index] + "</Folder>");
                string ss = "<Folder>\r\n<name>" + keyPlacemarks.Keys[index].Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;").ToString() +"</name>" + keyPlacemarks[index].ToString().Replace(">,<", "><") + "</Folder>";
               
                tempFolder = ss.ToString();
                rearrangedFolder = rearrangedFolder + tempFolder.ToString();
            }
           


            //Saving the KML Doc
            string finalxml = xmlHead + finalStyle + rearrangedFolder+ xmlTail;
            //File.WriteAllText("foo.xml", myss, Encoding.ASCII);
            XmlDocument xdocWrite = new XmlDocument();
            xdocWrite.LoadXml(finalxml);
           string outputfile = txtbxOutput.Text.ToString();
           //xdocWrite.Save("mfile.kml");
           xdocWrite.Save(outputfile);
           label1.Text = "Done!";

           MessageBox.Show("Success!");


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
