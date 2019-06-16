using System.Collections.Generic;

namespace Cat.Web.Infrastructure.Grids
{
    public class Grids
    {
        private static readonly List<GridScheme> _schemes = new List<GridScheme>();

        static Grids()
        {
            AddSchemeUsers();
        }

        public static string GetSchemesJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(_schemes);
        }

        private static void AddSchemeUsers()
        {
            var scheme = new GridScheme("grid_users");

            scheme.Columns.Add(new GridSchemeColumn
            {
                ColumnId = "UserName", 
                Caption = "User Name",
                Sortable = true,
                Searchable = true
            });
            scheme.Columns.Add(new GridSchemeColumn
            {
                ColumnId = "Email",
                Caption = "Email",
                Sortable = true,
                Searchable = true
            });
            scheme.Columns.Add(new GridSchemeColumn
            {
                ColumnId = "RolesView",
                Caption = "Roles",
            });
            scheme.Columns.Add(new GridSchemeColumn
            {
                ColumnId = "Actions", 
                Caption = "", 
                Type = CellType.Actions, 
                Actions = new List<GridSchemeAction>
                {
                    new GridSchemeAction
                    {
                        Name = "Edit", 
                        ButtonIconSet = "fas", 
                        ButtonIcon = "fa-user-edit",
                        ButtonColor = "primary",
                        TooltipText = "Edit user"
                    }, 
                    new GridSchemeAction
                    {
                        Name = "Remove",
                        ButtonIconSet = "fas", 
                        ButtonIcon = "fa-user-slash",
                        ButtonColor = "primary",
                        TooltipText = "Remove user"
                    }
                }
            });

            _schemes.Add(scheme);
        }
    }
}