using System;
using System.Collections.Generic;

namespace Cat.Web.Infrastructure.Grids
{
    public class Grids
    {
        static Grids()
        {
            SchemesJson = Newtonsoft.Json.JsonConvert.SerializeObject(GetSchemes());
        }

        public static string SchemesJson { get; }

        #region Grids declaration
        private static void RegisterGridSchemes(List<GridScheme> schemes)
        {
            AddSchemeUsers(schemes);
        }

        private static void AddSchemeUsers(List<GridScheme> schemes)
        {
            AddScheme("grid_users", schemes, (scheme) =>
            {

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

            });
        }
        #endregion

        private static void AddScheme(string schemeName, List<GridScheme> schemes, Action<GridScheme> initGridScheme)
        {
            var scheme = new GridScheme(schemeName);
            initGridScheme(scheme);
            schemes.Add(scheme);
        }

        private static List<GridScheme> GetSchemes()
        {
            var schemes = new List<GridScheme>();
            RegisterGridSchemes(schemes);
            return schemes;
        }
    }
}