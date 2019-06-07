
using System.Collections.Generic;

namespace Cat.Web.Infrastructure.Grids
{
    public class GridScheme
    {
        public GridScheme(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public List<GridSchemeColumn> Columns = new List<GridSchemeColumn>();
    }

    public class GridSchemeColumn
    {
        public GridSchemeColumn()
        {
            Sortable = false;
            Visible = true;
            Searchable = false;
            Type = CellType.Data;
        }

        public string ColumnId { get; set; }

        public string Caption { get; set; }

        public bool Sortable { get; set; }

        public bool Visible { get; set; }

        public bool Searchable { get; set; }

        public CellType Type { get; set; }

        public List<GridSchemeAction> Actions;
    }

    public class GridSchemeAction
    {
        public string Name { get; set; }

        public string ButtonIconSet { get; set; }

        public string ButtonIcon { get; set; }

        public string ButtonColor { get; set; }

        public string TooltipText { get; set; }
    }

    public enum CellType
    {
        Unknown = 0,
        Data = 1,
        Actions = 2
    }
}