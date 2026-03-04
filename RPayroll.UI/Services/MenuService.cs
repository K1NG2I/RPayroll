using RPayroll.UI.Models;

namespace RPayroll.UI.Services;

public class MenuService
{
    public List<MenuItem> GetMenuItems(string? role)
    {
        var menus = BuildMenu();

        if (string.IsNullOrWhiteSpace(role))
        {
            return new List<MenuItem>();
        }

        var isAdmin = string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase);
        var isHr = string.Equals(role, "HR", StringComparison.OrdinalIgnoreCase);
        var isManager = string.Equals(role, "Manager", StringComparison.OrdinalIgnoreCase);

        var allowedIds = new HashSet<int>
        {
            1,
            101,
            201,
            202,
            301,
            302,
            401
        };

        if (isAdmin || isHr || isManager)
        {
            allowedIds.Add(601);
            allowedIds.Add(602);
        }

        if (isAdmin || isHr)
        {
            allowedIds.Add(203);
            allowedIds.Add(204);
            allowedIds.Add(303);
            allowedIds.Add(402);
            allowedIds.Add(701);
        }

        return FilterMenus(menus, allowedIds);
    }

    private static List<MenuItem> FilterMenus(List<MenuItem> menus, HashSet<int> allowedIds)
    {
        var filtered = new List<MenuItem>();
        foreach (var menu in menus)
        {
            if (!allowedIds.Contains(menu.Id) && menu.ParentId != null)
            {
                continue;
            }

            var children = menu.Children
                .Where(child => allowedIds.Contains(child.Id))
                .OrderBy(child => child.DisplayOrder)
                .ToList();

            if (menu.ParentId == null && menu.Children.Count > 0 && children.Count == 0 && menu.Id != 1)
            {
                continue;
            }

            filtered.Add(new MenuItem
            {
                Id = menu.Id,
                Title = menu.Title,
                Controller = menu.Controller,
                Action = menu.Action,
                ParentId = menu.ParentId,
                Icon = menu.Icon,
                DisplayOrder = menu.DisplayOrder,
                Children = children
            });
        }

        return filtered.OrderBy(m => m.DisplayOrder).ToList();
    }

    private static List<MenuItem> BuildMenu()
    {
        return new List<MenuItem>
        {
            new()
            {
                Id = 1,
                Title = "Dashboard",
                Controller = "Home",
                Action = "Index",
                DisplayOrder = 1
            },
            new()
            {
                Id = 2,
                Title = "Profile",
                DisplayOrder = 2,
                Children = new List<MenuItem>
                {
                    new()
                    {
                        Id = 101,
                        Title = "My Profile",
                        Controller = "Employee",
                        Action = "MyProfile",
                        ParentId = 2,
                        DisplayOrder = 1
                    }
                }
            },
            new()
            {
                Id = 3,
                Title = "Attendance",
                DisplayOrder = 3,
                Children = new List<MenuItem>
                {
                    new()
                    {
                        Id = 201,
                        Title = "Mark Attendance",
                        Controller = "Attendance",
                        Action = "Marking",
                        ParentId = 3,
                        DisplayOrder = 1
                    },
                    new()
                    {
                        Id = 202,
                        Title = "Attendance",
                        Controller = "Attendance",
                        Action = "My",
                        ParentId = 3,
                        DisplayOrder = 2
                    },
                    new()
                    {
                        Id = 203,
                        Title = "Attendance Marking",
                        Controller = "Attendance",
                        Action = "Marking",
                        ParentId = 3,
                        DisplayOrder = 3
                    },
                    new()
                    {
                        Id = 204,
                        Title = "Attendance List",
                        Controller = "Attendance",
                        Action = "List",
                        ParentId = 3,
                        DisplayOrder = 4
                    }
                }
            },
            new()
            {
                Id = 4,
                Title = "Leave Management",
                DisplayOrder = 4,
                Children = new List<MenuItem>
                {
                    new()
                    {
                        Id = 301,
                        Title = "Apply Leave",
                        Controller = "Leave",
                        Action = "Apply",
                        ParentId = 4,
                        DisplayOrder = 1
                    },
                    new()
                    {
                        Id = 302,
                        Title = "Leave",
                        Controller = "Leave",
                        Action = "MyLeaves",
                        ParentId = 4,
                        DisplayOrder = 2
                    },
                    new()
                    {
                        Id = 303,
                        Title = "Leave Approval",
                        Controller = "Leave",
                        Action = "Approval",
                        ParentId = 4,
                        DisplayOrder = 3
                    }
                }
            },
            new()
            {
                Id = 5,
                Title = "Payroll",
                DisplayOrder = 5,
                Children = new List<MenuItem>
                {
                    new()
                    {
                        Id = 401,
                        Title = "Payroll",
                        Controller = "Payroll",
                        Action = "List",
                        ParentId = 5,
                        DisplayOrder = 1
                    },
                    new()
                    {
                        Id = 402,
                        Title = "Generate Payroll",
                        Controller = "Payroll",
                        Action = "Generate",
                        ParentId = 5,
                        DisplayOrder = 2
                    }
                }
            },
            new()
            {
                Id = 6,
                Title = "Employee Management",
                DisplayOrder = 6,
                Children = new List<MenuItem>
                {
                    new()
                    {
                        Id = 601,
                        Title = "Employees",
                        Controller = "Employee",
                        Action = "List",
                        ParentId = 6,
                        DisplayOrder = 1
                    },
                    new()
                    {
                        Id = 602,
                        Title = "Create Employee",
                        Controller = "Employee",
                        Action = "Create",
                        ParentId = 6,
                        DisplayOrder = 2
                    }
                }
            },
            new()
            {
                Id = 7,
                Title = "Administration",
                DisplayOrder = 7,
                Children = new List<MenuItem>
                {
                    new()
                    {
                        Id = 701,
                        Title = "Role Management",
                        Controller = "Role",
                        Action = "List",
                        ParentId = 7,
                        DisplayOrder = 1
                    }
                }
            }
        };
    }
}
