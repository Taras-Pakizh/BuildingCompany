using MVC_Practice.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Practice.Repository
{
    public class TabCreator
    {
        private List<TabItem> tabs = new List<TabItem>();

        public TabCreator(string user)
        {
            tabs = CreateDefaultTabs(user);
        }

        public void ChooseTab(string tab)
        {
            tabs.Where(x => x.name == tab).Single().Selected = true;
        }

        public IEnumerable<TabItem> GetTabs
        {
            get
            {
                return tabs;
            }
        }

        private List<TabItem> CreateDefaultTabs(string user)
        {
            var tabs = new List<TabItem>();

            if(user == "admin")
            {
                tabs = new List<TabItem>()
                {
                    new TabItem()
                    {
                        name = "Roles",
                        href = "/Roles/Index",
                        Selected = false
                    },
                    new TabItem()
                    {
                        name = "Users",
                        href = "/Users/Index",
                        Selected = false
                    }
                };
            }
            else if(user == "Storage man")
            {
                tabs = new List<TabItem>()
                {
                    new TabItem()
                    {
                        name = "Purchases",
                        href = "/Purchase/Index",
                        Selected = false
                    },
                    new TabItem()
                    {
                        name = "Storages",
                        href = "/Storage/Storages",
                        Selected = false
                    },
                    new TabItem()
                    {
                        name = "Shipment to storages",
                        href = "/Shipment/ShipmentToStorages",
                        Selected = false
                    },
                    new TabItem()
                    {
                        name = "Sendings",
                        href = "/Sending/Sendings",
                        Selected = false
                    },
                };
            }
            else if(user == "HR")
            {
                tabs = new List<TabItem>()
                {
                    new TabItem()
                    {
                        name = "Employees",
                        href = "/Employees/Index",
                        Selected = false
                    },
                    new TabItem()
                    {
                        name = "Orders",
                        href = "/EmployeeOrders/Index",
                        Selected = false
                    },
                    new TabItem()
                    {
                        name = "Positions",
                        href = "/Positions/Index",
                        Selected = false
                    },
                    new TabItem()
                    {
                        name = "Departments",
                        href = "/Departments/Index",
                        Selected = false
                    },
                    new TabItem()
                    {
                        name = "Order types",
                        href = "/OrderTypes/Index",
                        Selected = false
                    },
                };
            }
            else
            {
                throw new Exception("Such user dont exists");
            }

            return tabs;
        }

        
    }
}