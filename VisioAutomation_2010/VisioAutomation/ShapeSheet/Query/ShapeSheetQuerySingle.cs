using System.Collections.Generic;
using System.Linq;

namespace VisioAutomation.ShapeSheet.Query
{
    public class ShapeSheetQuerySingle
    {
        public CellColumnCollection Cells { get; }

        public ShapeSheetQuerySingle()
        {
            this.Cells = new CellColumnCollection(0);
        }

        public CellColumn AddCell(ShapeSheet.Src src, string name)
        {
            if (name == null)
            {
                throw new System.ArgumentNullException(nameof(name));
            }

            var col = this.Cells.Add(src, name);
            return col;
        }

        private static void RestrictToShapesOnly(SurfaceTarget surface)
        {
            if (surface.Shape == null)
            {
                string msg = "Target must be Shape not Page or Master";
                throw new System.ArgumentException(msg);
            }
        }

        public QueryOutput<string> GetFormulas(Microsoft.Office.Interop.Visio.Shape shape)
        {
            var surface = new SurfaceTarget(shape);
            return GetFormulas(surface);
        }

        public QueryOutput<string> GetFormulas(SurfaceTarget surface)
        {
            RestrictToShapesOnly(surface);

            var shapes = new List<Microsoft.Office.Interop.Visio.Shape> { surface.Shape };

            var srcstream = this._build_src_stream();
            var values = surface.GetFormulasU(srcstream);
            var shape_index = 0;
            var seg_builder = new VisioAutomation.Utilities.ArraySegmentReader<string>(values);
            var output_for_shape = this._create_output_for_shape(surface.ID16, null, seg_builder);

            return output_for_shape;
        }

        public QueryOutput<TResult> GetResults<TResult>(Microsoft.Office.Interop.Visio.Shape shape)
        {
            var surface = new SurfaceTarget(shape);
            return GetResults<TResult>(surface);
        }

        public QueryOutput<TResult> GetResults<TResult>(SurfaceTarget surface)
        {
            RestrictToShapesOnly(surface);

            var shapes = new List<Microsoft.Office.Interop.Visio.Shape> { surface.Shape };

            var srcstream = this._build_src_stream();
            const object[] unitcodes = null;
            var values = surface.GetResults<TResult>(srcstream, unitcodes);
            var shape_index = 0;
            var seg_builder = new VisioAutomation.Utilities.ArraySegmentReader<TResult>(values);
            var output_for_shape = this._create_output_for_shape(surface.ID16, null, seg_builder);
            return output_for_shape;
        }

        public QueryOutput<ShapeSheet.CellData> GetFormulasAndResults(Microsoft.Office.Interop.Visio.Shape shape)
        {
            var surface = new SurfaceTarget(shape);
            return this.GetFormulasAndResults(surface);
        }

        private static CellData[] _combine_formulas_and_results(string[] formulas, string[] results)
        {
            int n = results.Length;

            if (formulas.Length != results.Length)
            {
                throw new System.ArgumentException("Array Lengths must match");
            }

            var combined_data = new ShapeSheet.CellData[n];
            for (int i = 0; i < n; i++)
            {
                combined_data[i] = new ShapeSheet.CellData(formulas[i], results[i]);
            }
            return combined_data;
        }

        public QueryOutput<ShapeSheet.CellData> GetFormulasAndResults(SurfaceTarget surface)
        {
            RestrictToShapesOnly(surface);

            var shapes = new List<Microsoft.Office.Interop.Visio.Shape> { surface.Shape };

            var srcstream = this._build_src_stream();
            const object[] unitcodes = null;
            var formulas = surface.GetFormulasU(srcstream);
            var results = surface.GetResults<string>(srcstream, unitcodes);
            var combined_data = _combine_formulas_and_results(formulas, results);

            var seg_builder = new VisioAutomation.Utilities.ArraySegmentReader<CellData>(combined_data);
            var output_for_shape = this._create_output_for_shape(surface.ID16, null, seg_builder);
            return output_for_shape;
        }

        public QueryOutputCollection<string> GetFormulas(Microsoft.Office.Interop.Visio.Page page, IList<int> shapeids)
        {
            var surface = new SurfaceTarget(page);
            return this.GetFormulas(surface, shapeids);
        }

        public QueryOutputCollection<string> GetFormulas(SurfaceTarget surface, IList<int> shapeids)
        {
            var shapes = new List<Microsoft.Office.Interop.Visio.Shape>(shapeids.Count);
            shapes.AddRange(shapeids.Select(shapeid => surface.Shapes.ItemFromID16[(short)shapeid]));

            var srcstream = this._build_sidsrc_stream(shapeids);
            var values = surface.GetFormulasU(srcstream);
            var seg_builder = new VisioAutomation.Utilities.ArraySegmentReader<string>(values);
            var list = this._create_outputs_for_shapes(shapeids, null, seg_builder);
            return list;
        }

        public QueryOutputCollection<TResult> GetResults<TResult>(Microsoft.Office.Interop.Visio.Page page, IList<int> shapeids)
        {
            var surface = new SurfaceTarget(page);
            return this.GetResults<TResult>(surface, shapeids);
        }

        public QueryOutputCollection<TResult> GetResults<TResult>(SurfaceTarget surface, IList<int> shapeids)
        {
            var shapes = new List<Microsoft.Office.Interop.Visio.Shape>(shapeids.Count);
            shapes.AddRange(shapeids.Select(shapeid => surface.Shapes.ItemFromID16[(short)shapeid]));

            var srcstream = this._build_sidsrc_stream(shapeids);
            const object[] unitcodes = null;
            var values = surface.GetResults<TResult>(srcstream, unitcodes);
            var seg_builder = new VisioAutomation.Utilities.ArraySegmentReader<TResult>(values);
            var list = this._create_outputs_for_shapes(shapeids, null, seg_builder);
            return list;
        }

        public QueryOutputCollection<ShapeSheet.CellData> GetFormulasAndResults(Microsoft.Office.Interop.Visio.Page page, IList<int> shapeids)
        {
            var surface = new SurfaceTarget(page);
            return this.GetFormulasAndResults(surface, shapeids);
        }

        public QueryOutputCollection<ShapeSheet.CellData> GetFormulasAndResults(SurfaceTarget surface, IList<int> shapeids)
        {
            var shapes = new List<Microsoft.Office.Interop.Visio.Shape>(shapeids.Count);
            shapes.AddRange(shapeids.Select(shapeid => surface.Shapes.ItemFromID16[(short)shapeid]));

            var srcstream = this._build_sidsrc_stream(shapeids);
            const object[] unitcodes = null;
            var results = surface.GetResults<string>(srcstream, unitcodes);
            var formulas = surface.GetFormulasU(srcstream);
            var combined_data = _combine_formulas_and_results(formulas, results);

            var seg_builder = new VisioAutomation.Utilities.ArraySegmentReader<CellData>(combined_data);
            var r = this._create_outputs_for_shapes(shapeids, null, seg_builder);
            return r;
        }

        private QueryOutputCollection<T> _create_outputs_for_shapes<T>(IList<int> shapeids, SectionInfoCache cache, VisioAutomation.Utilities.ArraySegmentReader<T> segReader)
        {
            var output_for_all_shapes = new QueryOutputCollection<T>();

            for (int shape_index = 0; shape_index < shapeids.Count; shape_index++)
            {
                var shapeid = shapeids[shape_index];
                var subqueryinfo = this.GetSectionInfoForShape(shape_index, cache);
                var output_for_shape = this._create_output_for_shape((short)shapeid, subqueryinfo, segReader);
                output_for_all_shapes.Add(output_for_shape);
            }

            return output_for_all_shapes;
        }

        private List<SectionInfo> GetSectionInfoForShape(int shape_index, SectionInfoCache cache)
        {
            if (cache.CountShapes > 0)
            {
                return cache.GetSectionInfosForShapeAtIndex(shape_index);
            }
            return null;
        }

        private QueryOutput<T> _create_output_for_shape<T>(short shapeid, List<SectionInfo> section_infos, VisioAutomation.Utilities.ArraySegmentReader<T> segReader)
        {
            int original_seg_size = segReader.Count;

            var output = new QueryOutput<T>(shapeid);

            // Figure out the total cell count for this shape
            output.TotalCellCount = this.Cells.Count;
            if (section_infos != null)
            {
                output.TotalCellCount += section_infos.Select(x => x.RowCount * x.SubQuery.Columns.Count).Sum();
            }

            // First Copy the Query Cell Values into the output
            output.Cells = segReader.GetNextSegment(this.Cells.Count); ;

            // Now copy the Section values over
            if (section_infos != null)
            {
                output.Sections = new List<SubQueryOutput<T>>(section_infos.Count);
                foreach (var section_info in section_infos)
                {
                    var subquery_output = new SubQueryOutput<T>(section_info.RowCount, section_info.SubQuery.SectionIndex);

                    int num_cols = section_info.SubQuery.Columns.Count;
                    foreach (int row_index in section_info.RowIndexes)
                    {
                        var segment = segReader.GetNextSegment(num_cols);
                        var sec_res_row = new SubQueryOutputRow<T>(segment, section_info.SubQuery.SectionIndex, row_index);
                        subquery_output.Rows.Add(sec_res_row);
                    }

                    output.Sections.Add(subquery_output);
                }
            }

            int final_seg_size = segReader.Count;

            if ((final_seg_size - original_seg_size) != output.TotalCellCount)
            {
                throw new VisioAutomation.Exceptions.InternalAssertionException("Unexpected cursor");
            }

            return output;
        }

        private int _get_total_cell_count(int numshapes)
        {
            // Count the cells not in sections
            int count = this.Cells.Count * numshapes;

            // Count the Cells in the Sections
            return count;
        }

        private Streams.StreamArray _build_src_stream()
        {
            int dummy_shapeid = -1;
            int numshapes = 1;
            int shapeindex = 0;
            int numcells = this._get_total_cell_count(numshapes);
            var stream = new VisioAutomation.ShapeSheet.Streams.FixedSrcStreamBuilder(numcells);
            var cellinfos = this._enum_total_cellinfo(dummy_shapeid, shapeindex);
            var srcs = cellinfos.Select(i => i.SidSrc.Src);
            stream.AddRange(srcs);

            return stream.ToStream();
        }

        private VisioAutomation.ShapeSheet.Streams.StreamArray _build_sidsrc_stream(IList<int> shapeids)
        {
            int numshapes = shapeids.Count;
            int numcells = this._get_total_cell_count(numshapes);

            var stream = new VisioAutomation.ShapeSheet.Streams.FixedSidSrcStreamBuilder(numcells);

            for (int shapeindex = 0; shapeindex < shapeids.Count; shapeindex++)
            {
                // For each shape add the cells to query
                var shapeid = shapeids[shapeindex];

                var cellinfos = this._enum_total_cellinfo(shapeid, shapeindex);
                var sidsrcs = cellinfos.Select(i => i.SidSrc);
                stream.AddRange(sidsrcs);
            }

            return stream.ToStream();
        }

        private IEnumerable<Internal.QueryCellInfo> _enum_total_cellinfo(int shapeid, int shapeindex)
        {
            // enum Cells
            foreach (var col in this.Cells)
            {
                var sidsrc = new SidSrc((short)shapeid, col.Src);

                var cellinfo = new Internal.QueryCellInfo(sidsrc, col);
                yield return cellinfo;
            }
        }
    }
}