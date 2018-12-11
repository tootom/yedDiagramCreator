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
            int xspacing = 250;
            int yspacing_meters = 80;
            int yspacing_SPID = 160;
            int col = 0;
            int row = 0;
            yedDiagram yed = new yedDiagram();
            var doc = yed.LoadTemplate();

            yed.addGroupingNode(doc, "1", "Site", 220, 0);
            yed.addSVGnode(doc, "2", "Test Water","WSPID",  0);
            yed.addSVGnode(doc, "3", "Sewerage SpiD", "SSPID",500);
            yed.addSVGnode(doc, "4", "M1" + Environment.NewLine + "YVE:30m3", "POTABLE" ,250, yspacing_meters*row++, "1");
            yed.addSVGnode(doc, "5", "M2" + Environment.NewLine + "YVE:500m3", "PRIVATEWATER",250, yspacing_meters * row++,"1");
            yed.addSVGnode(doc, "6", "M3" + Environment.NewLine + "YVE:500m3", "PRIVATEWATER", 250, yspacing_meters * row++, "1");
            yed.addSVGnode(doc, "7", "M2" + Environment.NewLine + "YVE:500m3", "PRIVATEWATER", 250, yspacing_meters * row++, "1");
            yed.addSVGnode(doc, "8", "M3" + Environment.NewLine + "YVE:500m3", "PRIVATEWATER", 250, yspacing_meters * row++, "1");

            yed.addEdge(doc, "2", "4", null);
            yed.addEdge(doc, "5", "3", "RTS 100");

            yed.addEdge(doc, "4", "3", "RTS 50%");
            yed.SaveFile(doc, "C:\\Users\\Thomas\\Yed Diagram\\testing4.graphml");
            
        }
    }
}
