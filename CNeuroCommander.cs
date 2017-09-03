using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace NeuroLib
{
    public class CNeuroCommander
    {
        public CNeuroCommander()
        {

        }
        public CANN CreateANN(string fileName, int[] arhitect, double[,] inputs_minmax, double[,] outputs_minmax)
        {
            CANN myANN = new CANN(arhitect, inputs_minmax, outputs_minmax);

            BinaryFormatter binFormat = new BinaryFormatter();
            using (Stream fStream = new FileStream(fileName,
                   FileMode.Create, FileAccess.Write, FileShare.None))
            {
                binFormat.Serialize(fStream, myANN);
            }
            return myANN;
        }
        public CANN CreateANN(string fileName)
        {
            CANN annFromDisk;
            BinaryFormatter binFormat = new BinaryFormatter();
            using (Stream fStream = File.OpenRead(fileName))
            {
                annFromDisk =
                  (CANN)binFormat.Deserialize(fStream);

            }

            return annFromDisk;
        }

        public void SaveToXml(CANN Ann, string fileName)
        {
            XmlTextWriter textWritter = new XmlTextWriter(fileName, Encoding.UTF8);
            textWritter.WriteStartDocument();
            textWritter.WriteStartElement("ann");
            
            textWritter.WriteEndElement();
            textWritter.Close();

            XmlDocument document = new XmlDocument();
            document.Load(fileName);

            

            foreach (var r in Ann._inputMinMax)
            {
                XmlNode element = document.CreateElement("border");
                document.DocumentElement.AppendChild(element);

                XmlAttribute attribute = document.CreateAttribute("type"); // создаём атрибут
                attribute.Value = "input"; // устанавливаем значение атрибута
                element.Attributes.Append(attribute);

                attribute = document.CreateAttribute("min"); // создаём атрибут
                attribute.Value = "input"; // устанавливаем значение атрибута
                element.Attributes.Append(attribute);

            }


            for (int i = 0; i < Ann._Net.Length; i++)
                for (int j = 0; j < Ann._Net[i].Length; j++)
                {
                    XmlNode element = document.CreateElement("neuron");
                    document.DocumentElement.AppendChild(element); // указываем родителя

                    XmlAttribute attribute = document.CreateAttribute("layer"); // создаём атрибут
                    attribute.Value = i.ToString(); // устанавливаем значение атрибута
                    element.Attributes.Append(attribute); // добавляем атрибут

                    attribute = document.CreateAttribute("number"); // создаём атрибут
                    attribute.Value = j.ToString(); // устанавливаем значение атрибута
                    element.Attributes.Append(attribute); // добавляем атрибут

                    attribute = document.CreateAttribute("actfunc"); // создаём атрибут
                    if (Ann._Net[i][j]._actFunc != null)
                        attribute.Value = Ann._Net[i][j]._actFunc.ToString(); // устанавливаем значение атрибута
                    else
                        attribute.Value = "null";
                    element.Attributes.Append(attribute); // добавляем атрибут

                    if (Ann._Net[i][j]._Weights != null)
                        for (int k = 0; k < Ann._Net[i][j]._Weights.Length; k++)
                        {
                            XmlNode subElement1 = document.CreateElement("weight"); // даём имя
                            subElement1.InnerText = Ann._Net[i][j]._Weights[k].ToString(); // и значение
                            element.AppendChild(subElement1); // и указываем кому принадлежит
                        }

                }

            document.Save(fileName);
        }

        public CANN CreateFromXml(string fileName)
        {
            if (File.Exists(fileName)&& Path.GetExtension(fileName)==".xml")
            {
                 XDocument xdoc = XDocument.Load(fileName);
                
                int NetLayers= xdoc.Root.Elements("neuron").Max(e=>(int)e.Attribute("layer"))+1;
                int[] arh = new int[NetLayers];
                          

                foreach (XElement neuron in xdoc.Element("ann").Elements("neuron"))
                {

                }
            }

            return null;
        }
    }
}
