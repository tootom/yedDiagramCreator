using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
namespace yedDiagramCreator
{
    class yedDiagram
    {
      public static XNamespace graphml = "http://graphml.graphdrawing.org/xmlns";
      public static XNamespace java = "http://www.yworks.com/xml/yfiles-common/1.0/java";
      public static XNamespace sys = "http://www.yworks.com/xml/yfiles-common/markup/primitives/2.0";
      public static XNamespace x = "http://www.yworks.com/xml/yfiles-common/markup/2.0";
      public static XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
      public static XNamespace y = "http://www.yworks.com/xml/graphml";
      public static XNamespace yed = "http://www.yworks.com/xml/yed/3";
        XmlNamespaceManager mngr = new XmlNamespaceManager(new NameTable());


        void  AddNamespaces() {
           
 
         mngr.AddNamespace( "graphml" , "http://graphml.graphdrawing.org/xmlns");
         mngr.AddNamespace( "java" , "http://www.yworks.com/xml/yfiles-common/1.0/java");
         mngr.AddNamespace( "sys", "http://www.yworks.com/xml/yfiles-common/markup/primitives/2.0");
         mngr.AddNamespace( "x" ,"http://www.yworks.com/xml/yfiles-common/markup/2.0");
         mngr.AddNamespace( "xsi", "http://www.w3.org/2001/XMLSchema-instance");
         mngr.AddNamespace( "y" , "http://www.yworks.com/xml/graphml");
            mngr.AddNamespace("yed" , "http://www.yworks.com/xml/yed/3");
                
        }
         public XDocument LoadTemplate()
        {
            AddNamespaces();
            XDocument doc = XDocument.Load("Resources\\Template Blank.graphml");
            return doc;
           
        }

         public void SaveFile(XDocument doc, string filepath)
        {
            removeEmptyXMLNS(doc);
            doc.Save(filepath);
        }
         public void addNode(XDocument doc, string nodeID, string nodetext  )
        {
          var root = doc.Descendants(graphml + "graph").Last();

            XElement n = new XElement(graphml + "node");
            n.Add(new XAttribute("id", nodeID));
            n.SetValue(nodetext);
            root.Add(n);

        }

         public void addGroupingNode(XDocument doc, 
            string nodeID,
            string nodetext, 
            int? x_pos = 0, 
            int? y_pos = 0,
            int? height = 200,
            int? width = 200)
        {
            var root = doc.Descendants(graphml + "graph").Last();
            string textTemlate = System.IO.File.ReadAllText(@"Resources\\Group Node Template.graphml");
            string xmltext = String.Format(textTemlate, nodeID, width ??100, height??100, x_pos??0, y_pos??0, nodetext);


            XElement n = ParseElement(xmltext, mngr);
            

            root.Add(n);
          //  removeEmptyXMLNS(n);
        }


        void removeEmptyXMLNS(XDocument doc)
        {
            foreach (var node in doc.Root.Descendants())
            {
                // If we have an empty namespace...
                if (node.Name.NamespaceName == "")
                {
                    // Remove the xmlns='' attribute. Note the use of
                    // Attributes rather than Attribute, in case the
                    // attribute doesn't exist (which it might not if we'd
                    // created the document "manually" instead of loading
                    // it from a file.)
                    node.Attributes("xmlns").Remove();
                    // Inherit the parent namespace instead
                    node.Name = node.Parent.Name.Namespace + node.Name.LocalName;
                }
            }
        }

        /// <summary>Same as XElement.Parse(), but supports XML namespaces.</summary>
        /// <param name="strXml">A String that contains XML.</param>
        /// <param name="mngr">The XmlNamespaceManager to use for looking up namespace information.</param>
        /// <returns>An XElement populated from the string that contains XML.</returns>
        public  XElement ParseElement(string strXml, XmlNamespaceManager mngr)
        {
            XmlParserContext parserContext = new XmlParserContext(null, mngr, null, XmlSpace.None);
            XmlTextReader txtReader = new XmlTextReader(strXml, XmlNodeType.Element, parserContext);
            return XElement.Load(txtReader);
        }

         public void addSVGnode(XDocument doc, string nodeID, string nodetext, string nodeType, int? x_pos = 0, int? y_pos = 0, string parentnodeID = null)
        {
            int height = 60;
            string[] tall = { "SSPID","WSPID","DPID"};
            if (tall.Contains(nodeType)){
                height = 150;
            }

            XElement root =null; 

            if (parentnodeID != null)
            {
                 root =
    doc.Descendants( "graph").
    SingleOrDefault(e => ((string)e.Attribute("id")) == parentnodeID + "_");
               
               // removeEmptyXMLNS(root);
            }
            if (root ==null) {

                root = doc.Descendants(graphml + "graph").Last();
            }
            XElement n = new XElement(graphml + "node");
            n.Add(new XAttribute("id", nodeID));

            XElement data = new XElement(graphml + "data");
            data.Add(new XAttribute("key", "d6"));
            n.AddFirst(data);
            XElement svg = new XElement(y + "SVGNode");
                data.Add(svg);
            XElement SVGModel =   new XElement(y + "SVGModel", new XAttribute("svgBoundsPolicy",0));
            svg.Add(SVGModel);
            XElement SVGContent = new XElement(y + "SVGContent", new XAttribute("refid", nodeType));
            SVGModel.Add(SVGContent);

            XElement Geometry = new XElement(y + "Geometry",
                new XAttribute("height", height),
                new XAttribute("width", 195),
                new XAttribute("x", x_pos??0),
                new XAttribute("y", y_pos??0));
            svg.Add(Geometry);
            XElement NodeLabel = new XElement(y + "NodeLabel", new XAttribute("svgBoundsPolicy", 0));
            NodeLabel.SetValue(nodetext);
            svg.Add(NodeLabel);
            root.Add(n);


            //  <data key="d6">
            //   <y:SVGNode>
            //     <y:Geometry height="149.5163116455078" width="195.23060607910156" x="241.0" y="338.0"/>
            //     <y:Fill color="#CCCCFF" transparent="false"/>
            //     <y:BorderStyle color="#000000" type="line" width="1.0"/>
            //     <y:NodeLabel alignment="center" autoSizePolicy="content" fontFamily="Dialog" fontSize="12" fontStyle="plain" hasBackgroundColor="false" hasLineColor="false" height="18.701171875" horizontalTextPosition="center" iconTextGap="4" modelName="sandwich" modelPosition="s" textColor="#000000" verticalTextPosition="bottom" visible="true" width="50.69921875" x="72.26569366455078" y="153.5163116455078">F2 Label</y:NodeLabel>
            //     <y:SVGNodeProperties usingVisualBounds="true"/>
            //     <y:SVGModel svgBoundsPolicy="0">
            //       <y:SVGContent refid="6"/>
            //     </y:SVGModel>
            //   </y:SVGNode>
            // </data>

        }


         public void addEdge(XDocument doc,string SourceID,string TargetID, string edgeText) {
            var root = doc.Descendants(graphml + "graph").Last();
            XElement n = new XElement(graphml + "edge", new XAttribute("source", SourceID), new XAttribute("target", TargetID));
            n.Add();
       
            root.Add(n);

            XElement data = new XElement(graphml + "data");
            data.Add(new XAttribute("key", "d10"));
            n.AddFirst(data);

            XElement PolyLineEdge = new XElement(y + "PolyLineEdge");
            data.Add(PolyLineEdge);
            if (edgeText != null)
            {
                XElement EdgeLabel = new XElement(y + "EdgeLabel");//, new XAttribute("backgroundColor", "#FFFFFF"), new XAttribute("lineColor", "#000000"));
                EdgeLabel.SetValue(edgeText);
                PolyLineEdge.Add(EdgeLabel);
            }

            XElement Arrows = new XElement(y + "Arrows", new XAttribute("source", "none"), new XAttribute("target", "standard"));

            PolyLineEdge.Add(Arrows);

            // <edge id="e1" source="n0" target="n3">
            //      <data key="d10">1.0</data>
            //      <data key="d12"/>
            //      <data key="d13">
            //        <y:PolyLineEdge>
            //          <y:Path sx="-26.666666666666657" sy="45.0" tx="0.0" ty="-45.0">
            //            <y:Point x="198.33333333333334" y="100.0"/>
            //            <y:Point x="60.0" y="100.0"/>
            //          </y:Path>
            //          <y:LineStyle color="#000000" type="line" width="1.0"/>
            //          <y:Arrows source="none" target="standard"/>
            //          <y:EdgeLabel alignment="center" backgroundColor="#FFFFFF" configuration="AutoFlippingLabel" distance="2.0" fontFamily="Dialog" fontSize="12" fontStyle="plain" height="18.701171875" horizontalTextPosition="center" iconTextGap="4" lineColor="#000000" modelName="custom" preferredPlacement="anywhere" ratio="0.5" textColor="#000000" verticalTextPosition="bottom" visible="true" width="55.36328125" x="-96.84830220540364" y="0.6494140625">EdgeText<y:LabelModel>
            //              <y:SmartEdgeLabelModel autoRotationEnabled="false" defaultAngle="0.0" defaultDistance="10.0"/>
            //            </y:LabelModel>
            //            <y:ModelParameter>
            //              <y:SmartEdgeLabelModelParameter angle="0.0" distance="30.0" distanceToCenter="true" position="center" ratio="0.5" segment="1"/>
            //            </y:ModelParameter>
            //            <y:PreferredPlacementDescriptor angle="0.0" angleOffsetOnRightSide="0" angleReference="absolute" angleRotationOnRightSide="co" distance="-1.0" frozen="true" placement="anywhere" side="anywhere" sideReference="relative_to_edge_flow"/>
            //          </y:EdgeLabel>
            //          <y:BendStyle smoothed="false"/>
            //        </y:PolyLineEdge>
            //      </data>
            //    </edge>
        }
    }
}
