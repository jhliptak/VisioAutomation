using System.Collections.Generic;
using VASS=VisioAutomation.ShapeSheet;
using VisioAutomation.ShapeSheet.CellGroups;
using IVisio = Microsoft.Office.Interop.Visio;

namespace VisioAutomation.Shapes
{
    public class HyperlinkCells : CellGroup
    {
        public VASS.CellValueLiteral Address { get; set; }
        public VASS.CellValueLiteral Description { get; set; }
        public VASS.CellValueLiteral ExtraInfo { get; set; }
        public VASS.CellValueLiteral Frame { get; set; }
        public VASS.CellValueLiteral SortKey { get; set; }
        public VASS.CellValueLiteral SubAddress { get; set; }
        public VASS.CellValueLiteral NewWindow { get; set; }
        public VASS.CellValueLiteral Default { get; set; }
        public VASS.CellValueLiteral Invisible { get; set; }

        public override IEnumerable<CellMetadataItem> CellMetadata
        {
            get
            {
                yield return CellMetadataItem.Create(nameof(this.Address), VASS.SrcConstants.HyperlinkAddress, this.Address);
                yield return CellMetadataItem.Create(nameof(this.Description), VASS.SrcConstants.HyperlinkDescription, this.Description);
                yield return CellMetadataItem.Create(nameof(this.ExtraInfo), VASS.SrcConstants.HyperlinkExtraInfo, this.ExtraInfo);
                yield return CellMetadataItem.Create(nameof(this.Frame), VASS.SrcConstants.HyperlinkFrame, this.Frame);
                yield return CellMetadataItem.Create(nameof(this.SortKey), VASS.SrcConstants.HyperlinkSortKey, this.SortKey);
                yield return CellMetadataItem.Create(nameof(this.SubAddress), VASS.SrcConstants.HyperlinkSubAddress, this.SubAddress);
                yield return CellMetadataItem.Create(nameof(this.NewWindow), VASS.SrcConstants.HyperlinkNewWindow, this.NewWindow);
                yield return CellMetadataItem.Create(nameof(this.Default), VASS.SrcConstants.HyperlinkDefault, this.Default);
                yield return CellMetadataItem.Create(nameof(this.Invisible), VASS.SrcConstants.HyperlinkInvisible, this.Invisible);
            }
        }

        public static List<List<HyperlinkCells>> GetCells(IVisio.Page page, IList<int> shapeids, VASS.CellValueType type)
        {
            var reader = HyperLinkCells_lazy_builder.Value;
            return reader.GetCellsMultiRow(page, shapeids, type);
        }

        public static List<HyperlinkCells> GetCells(IVisio.Shape shape, VASS.CellValueType type)
        {
            var reader = HyperLinkCells_lazy_builder.Value;
            return reader.GetCellsMultiRow(shape, type);
        }

        private static readonly System.Lazy<HyperlinkCellsBuilder> HyperLinkCells_lazy_builder = new System.Lazy<HyperlinkCellsBuilder>();


        class HyperlinkCellsBuilder : CellGroupBuilder<HyperlinkCells>
        {

            public HyperlinkCellsBuilder() : base(CellGroupBuilderType.MultiRow)
            {
            }

            public override HyperlinkCells ToCellGroup(ShapeSheet.Internal.ArraySegment<string> row, VisioAutomation.ShapeSheet.Query.ColumnList cols)
            {
                var cells = new HyperlinkCells();

                string getcellvalue(string name)
                {
                    return row[cols[name].Ordinal];
                }


                cells.Address = getcellvalue(nameof(HyperlinkCells.Address));
                cells.Description = getcellvalue(nameof(HyperlinkCells.Description));
                cells.ExtraInfo = getcellvalue(nameof(HyperlinkCells.ExtraInfo));
                cells.Frame = getcellvalue(nameof(HyperlinkCells.Frame));
                cells.SortKey = getcellvalue(nameof(HyperlinkCells.SortKey));
                cells.SubAddress = getcellvalue(nameof(HyperlinkCells.SubAddress));
                cells.NewWindow = getcellvalue(nameof(HyperlinkCells.NewWindow));
                cells.Default = getcellvalue(nameof(HyperlinkCells.Default));
                cells.Invisible = getcellvalue(nameof(HyperlinkCells.Invisible));

                return cells;
            }
        }


    }
}