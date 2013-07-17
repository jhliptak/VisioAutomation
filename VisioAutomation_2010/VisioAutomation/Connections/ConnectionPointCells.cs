using VA=VisioAutomation;
using System.Collections.Generic;
using IVisio = Microsoft.Office.Interop.Visio;
using System.Linq;
using VisioAutomation.Extensions;

namespace VisioAutomation.Connections
{
    public class ConnectionPointCells : VA.ShapeSheet.CellGroups.CellGroupMultiRow
    {
        public VA.ShapeSheet.CellData<double> X { get; set; }
        public VA.ShapeSheet.CellData<double> Y { get; set; }
        public VA.ShapeSheet.CellData<int> DirX { get; set; }
        public VA.ShapeSheet.CellData<int> DirY { get; set; }
        public VA.ShapeSheet.CellData<int> Type { get; set; }

        public override void ApplyFormulasForRow(ApplyFormula func, short row)
        {
            func(VA.ShapeSheet.SRCConstants.Connections_X.ForRow(row), this.X.Formula);
            func(VA.ShapeSheet.SRCConstants.Connections_Y.ForRow(row), this.Y.Formula);
            func(VA.ShapeSheet.SRCConstants.Connections_DirX.ForRow(row), this.DirX.Formula);
            func(VA.ShapeSheet.SRCConstants.Connections_DirY.ForRow(row), this.DirY.Formula);
            func(VA.ShapeSheet.SRCConstants.Connections_Type.ForRow(row), this.Type.Formula);
        }
        
        public static IList<List<ConnectionPointCells>> GetCells(IVisio.Page page, IList<int> shapeids)
        {
            var query = get_query();
            return _GetCells(page, shapeids, query, query.GetCells);
        }

        public static IList<ConnectionPointCells> GetCells(IVisio.Shape shape)
        {
            var query = get_query();
            return _GetCells(shape, query, query.GetCells);
        }

        private static ConnectionPointCellQuery _mCellQuery;

        private static ConnectionPointCellQuery get_query()
        {
            _mCellQuery =  _mCellQuery ?? new ConnectionPointCellQuery();
            return _mCellQuery;
        }

        class ConnectionPointCellQuery : VA.ShapeSheet.Query.CellQuery
        {
            public VA.ShapeSheet.Query.CellQuery.Column DirX { get; set; }
            public VA.ShapeSheet.Query.CellQuery.Column DirY { get; set; }
            public VA.ShapeSheet.Query.CellQuery.Column Type { get; set; }
            public VA.ShapeSheet.Query.CellQuery.Column X { get; set; }
            public VA.ShapeSheet.Query.CellQuery.Column Y { get; set; }
            
            public ConnectionPointCellQuery()
            {
                var sec = this.Sections.Add(IVisio.VisSectionIndices.visSectionConnectionPts);
                DirX = sec.Columns.Add(VA.ShapeSheet.SRCConstants.Connections_DirX, "DirX");
                DirY = sec.Columns.Add(VA.ShapeSheet.SRCConstants.Connections_DirY, "DirY");
                Type = sec.Columns.Add(VA.ShapeSheet.SRCConstants.Connections_Type, "Type");
                X = sec.Columns.Add(VA.ShapeSheet.SRCConstants.Connections_X, "X");
                Y = sec.Columns.Add(VA.ShapeSheet.SRCConstants.Connections_Y, "Y");
            }

            public ConnectionPointCells GetCells(VA.ShapeSheet.CellData<double>[] row)
            {
                var cells = new ConnectionPointCells();
                cells.X = row[this.X.Ordinal];
                cells.Y = row[this.Y.Ordinal];
                cells.DirX = row[this.DirX.Ordinal].ToInt();
                cells.DirY = row[this.DirY.Ordinal].ToInt();
                cells.Type = row[this.Type.Ordinal].ToInt();

                return cells;
            }
        }
    }
}