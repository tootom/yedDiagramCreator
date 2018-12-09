using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace yedDiagramCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            var doc = yedDiagram.LoadTemplate();

            yedDiagram.addSVGnode(doc, "2", "Test Water","WSPID");
            yedDiagram.addSVGnode(doc, "3", "Sewerage SpiD", "SSPID");
            yedDiagram.addSVGnode(doc, "4", "M1" + Environment.NewLine + "YVE:30m3", "POTABLE");
            yedDiagram.addSVGnode(doc, "5", "M2" + Environment.NewLine + "YVE:500m3", "PRIVATEWATER");

            yedDiagram.addEdge(doc, "2", "4", null);
            yedDiagram.addEdge(doc, "5", "3", "RTS 100%");

            yedDiagram.addEdge(doc, "4", "3", "RTS 50%");
            yedDiagram.SaveFile(doc, "C:\\Users\\Thomas\\Yed Diagram\\testing2.graphml");
            
        }
    }
}
