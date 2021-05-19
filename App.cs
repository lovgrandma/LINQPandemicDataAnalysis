using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F_W2021_991577657
{
    /**
     * Will contain majority of functionality for final examination
     * 
     * Author Jesse Thompson
     */
    class App
    {
        public void runFinal()
        {
            bool running = true;
            while (running)
            {
                int select = mainMenuQuery();
                if (select == 1)
                {
                    getAllLocations();
                } else if (select == 2)
                {
                    addNewCovidCase();
                }
                else if (select == 3)
                {
                    addNewLocation();
                } else if (select == 4)
                {
                    updateLocation();
                }
                else if (select == 5)
                {
                    getCovidCasesByLocation();
                }
                else if (select == 6)
                {
                    getAllCovidCases();
                }
                else if (select == 7)
                {
                    running = false;
                }
            }
        }

        /**
         * Asks user where they would like to navigate on the menu. Returns retry value on error
         * 
         * returns {int} Value of choice 
         * */
        private int mainMenuQuery()
        {
            Console.WriteLine("1 - Get all Locations\n2 - Add New Covid Case\n3 - Add New Location\n4 - Update Location\n5 - Get All Covid cases by Location\n6 - Get all Covid Cases\n7 - Exit");
            try
            {
                return int.Parse(Console.ReadLine()); 
            } 
            catch (Exception err)
            {
                Console.WriteLine("You must enter a number to select a menu item");
                return -1;
            }
        }

        /**
         * Will print all locations from covid database
         * 
         * returns {void}
         */
        private void getAllLocations()
        {
            try
            {
                using (var context = new CovidDBEntities())
                {
                    // var locations = context.Locations.ToList();
                    var locations = (from location in context.Locations
                                     join covids in context.Covids
                                     on location.LocationId equals covids.locationId
                                     join continent in context.Continents
                                     on covids.continentId equals continent.ContinentId
                                     select new
                                     {
                                         locationId = location.LocationId,
                                         locationName = location.location,
                                         continentName = continent.continent,
                                         population = covids.population
                                     }).Distinct().ToList();
                    Console.WriteLine($"|{"Location Id",-12}|{"Location Name",-25}|{"Continent Name",-17}|{"Population",-12}|");
                    foreach (var item in locations)
                    {
                        String id = item.locationId.ToString();
                        String location = item.locationName;
                        String continent = item.continentName;
                        String population = item.population.ToString();
                        Console.Write($"|{id,-12}|{location,-25}|{continent,-17}|{population,-12}|\n");
                    }
                    Console.WriteLine("Done? (Press any key)");
                    Console.ReadLine();
                }
            } catch (Exception err)
            {
                Console.WriteLine("There was an error painting all locations");
                return;
            }
        }

        /**
         * Will print all covid cases
         * 
         * returns {void}
         */
        private void getAllCovidCases()
        {
            try
            {
                using (var context = new CovidDBEntities())
                {
                    var cases = (from covids in context.Covids
                                 join location in context.Locations
                                 on covids.locationId equals location.LocationId
                                 join continent in context.Continents
                                 on covids.continentId equals continent.ContinentId
                                 select new
                                 {
                                     covidId = covids.covidId,
                                     continentName = continent.continent,
                                     locationName = location.location,
                                     newCases = covids.new_cases,
                                     totalCases = covids.total_cases,
                                     population = covids.population,
                                     date = covids.date
                                 }).Distinct().ToList();
                    Console.WriteLine($"|{"CovidId",-8}|{"Continent Name",-17}|{"Location Name",-17}|{"New Cases",-7}|{"Total Cases",-9}|{"Population",-14}|{"Date",-10}|");
                    foreach (var item in cases)
                    {
                        String id = item.covidId.ToString();
                        String continentName = item.continentName;
                        String locationName = item.locationName;
                        String newCases = item.newCases.ToString();
                        if (newCases == "0") // When displaying New cases values, only show result if New Case is greater than zero value
                        {
                            newCases = "";
                        }
                        String totalCases = item.totalCases.ToString();
                        String population = item.population.ToString();
                        String date = item.date.ToString();
                        Console.Write($"|{id,-8}|{continentName,-17}|{locationName,-17}|{newCases,-7}|{totalCases,-9}|{population,-14}|{date,-10}\n");
                    }
                    Console.WriteLine("Done? (Press any key)");
                    Console.ReadLine();
                }
            } catch (Exception err)
            {
                Console.WriteLine("There was an error painting all covid cases");
                return;
            }
        }

        private void getContinentAndLocation(Boolean showIds = false)
        {
            try
            {
                using (var context = new CovidDBEntities())
                {
                    var continentIds = context.Continents.ToList();
                    Console.WriteLine($"|{"Continent Id",-12}|{"Continent",-17}|");
                    foreach (var item in continentIds)
                    {
                        String id = item.ContinentId.ToString();
                        String continent = item.continent;
                        Console.Write($"|{id,-12}|{continent,-17}|\n");
                    }
                    Console.WriteLine("Select an id to look at locations");
                    int select1 = getGoodNumber();
                    var locationsList = (from locations in context.Locations
                                         join covids in context.Covids
                                         on locations.LocationId equals covids.locationId
                                         join continent in context.Continents
                                         on covids.continentId equals continent.ContinentId
                                         select new
                                         {
                                             continentId = continent.ContinentId,
                                             locationId = locations.LocationId,
                                             location = locations.location,
                                             cases = covids.total_cases,
                                             covidId = covids.covidId,
                                             continentName = continent.continent,
                                             newCases = covids.new_cases,
                                             totalCases = covids.total_cases,
                                             population = covids.population,
                                             date = covids.date
                                         }
                        ).Where(l => l.continentId == select1).Distinct().ToList();
                    if (!showIds)
                    {
                        Console.WriteLine($"|{"CovidId",-7}|{"Continent Name",-17}|{"Location Name",-17}|{"New Cases",-7}|{"Total Cases",-9}|{"Population",-14}|{"Date",-10}|");
                    }
                    else
                    {
                        locationsList.GroupBy(o => o.locationId).Select(o => o.First()).DistinctBy(o => o.locationId).ToList();
                        Console.WriteLine($"|{"CovidId",-7}|{"Continent Name",-15}|{"Continent Id",-5}|{"Location Name",-15}|{"Location Id",-5}|{"New Cases",-7}|{"Total Cases",-7}|{"Population",-10}|{"Date",-10}|");
                    }
                    foreach (var item in locationsList)
                    {
                        String id = item.covidId.ToString();
                        String continentName = item.continentName;
                        String continentId = item.continentId.ToString();
                        String locationName = item.location;
                        String locationid = item.locationId.ToString();
                        String newCases = item.newCases.ToString();
                        if (newCases == "0") // When displaying New cases values, only show result if New Case is greater than zero value
                        {
                            newCases = "";
                        }
                        String totalCases = item.totalCases.ToString();
                        String population = item.population.ToString();
                        String date = item.date.ToString();
                        if (!showIds)
                        {
                            Console.Write($"|{id,-7}|{continentName,-17}|{locationName,-17}|{newCases,-7}|{totalCases,-9}|{population,-14}|{date,-10}\n");
                        }
                        else
                        {
                            Console.Write($"|{id,-7}|{continentName,-15}|{continentId,-5}|{locationName,-15}|{locationid,-5}|{newCases,-7}|{totalCases,-7}|{population,-10}|{date,-10}\n");
                        }
                    }
                    Console.WriteLine("Done? (Press any key)");
                    Console.ReadLine();
                }
            } catch (Exception err)
            {
                return;
            }
        }
        /**
         * Given a continent id, will return covid cases from that location
         * 
         * returns {void}
         */
        private void getCovidCasesByLocation()
        {
            try
            {
                Boolean getCases = true;
                while (getCases)
                {
                    getContinentAndLocation();
                    Console.WriteLine("Press anything to search again\nType -1 to exit");
                    String select2 = Console.ReadLine().ToString();
                    if (select2 == "-1")
                    {
                        getCases = false;
                        return;
                    }
                }
            } catch (Exception err)
            {
                return;
            }
        }

        /**
         * Will add a new location to the db. Creates location with id, name in Location table and location id, population, total cases and date from covid table
         * 
         * returns {void}
         */
        private void addNewLocation()
        {
            try
            {
                using (var context = new CovidDBEntities())
                {
                    Console.WriteLine("Choose the location id:");
                    int id = getGoodNumber();
                    Console.WriteLine("Enter a location name:");
                    String name = getGoodName();
                    var loc = new Location();
                    loc.LocationId = id;
                    loc.location = name;
                    context.Locations.Add(loc);
                    context.SaveChanges();
                }
            } catch (Exception err)
            {
                return;
            }
        }

        private void updateLocation()
        {
            try
            {
                using (var context = new CovidDBEntities())
                {
                    var locationsList = (from locations in context.Locations.Distinct()
                                         join covids in context.Covids.Distinct()
                                         on locations.LocationId equals covids.locationId
                                         select new
                                         {
                                             locationId = locations.LocationId,
                                             locationName = locations.location,
                                             population = covids.population,
                                             totalCases = covids.total_cases,
                                             date = covids.date
                                         }).DistinctBy(o => o.locationId).Distinct().ToList();
                    Console.WriteLine($"|{"Location Id",-12}|{"Location Name",-22}|{"Population",-17}|{"Total Cases",-13}|{"Date",-17}|");
                    foreach (var item in locationsList)
                    {
                        String id = item.locationId.ToString();
                        String locationName = item.locationName;
                        String totalCases = item.totalCases.ToString();
                        String population = item.population.ToString();
                        String date = item.date.ToString();
                        Console.Write($"|{id,-12}|{locationName,-22}|{population,-17}|{totalCases,-13}|{date, -17}|\n");
                    }
                    Console.WriteLine("Select the location that you want to update by inputting the id");

                    int select1 = getGoodNumber();
                    Console.WriteLine("What do you want to overwrite the name to");
                    String newName = getGoodName();                    
                    var locUpdate = context.Locations.Find(select1);
                    locUpdate.location = newName;
                    context.SaveChanges();
                }
            } catch (Exception err)
            {
                Console.WriteLine(err);
                return;
            }
        }

        private int getGoodNumber()
        {
            int select1 = 0;
            Boolean goodNumber = false;
            do
            {
                if (!int.TryParse(Console.ReadLine(), out select1))
                {
                    Console.WriteLine("You must enter a number");
                }
                else
                {
                    goodNumber = true;
                }
            } while (!goodNumber);
            return select1;
        }

        private String getGoodName()
        {
            String newName = Console.ReadLine();
            Boolean goodName = false;
            do
            {
                if (newName.Length <= 0)
                {
                    Console.WriteLine("You need to enter a valid name. Please enter below");
                    newName = Console.ReadLine();
                }
                else
                {
                    goodName = true;
                }
            } while (!goodName);
            return newName;
        }

        /**
         * Given the appropriate information, will add a new case to the db
         * 
         * returns {void}
         */
        private void addNewCovidCase()
        {
            try
            {
                getContinentAndLocation(true); // Show user the continent ids and location ids for selected continent as well as new cases
                Console.WriteLine("Adding a new covid case");
                Console.WriteLine("Enter the continent id");
                int continentId = getGoodNumber();
                Console.WriteLine("Enter the location id");
                int locationId = getGoodNumber();
                Console.WriteLine("Enter the new number of covid cases");
                int newCases = getGoodNumber();
                using (var context = new CovidDBEntities())
                {
                    var covid = new Covid();
                    covid.continentId = continentId;
                    covid.locationId = locationId;
                    covid.new_cases = newCases;
                    int totalCases = getCovidLast(continentId, locationId);
                    covid.total_cases = covid.total_cases + totalCases;
                    context.Covids.Add(covid);
                    context.SaveChanges();
                    Console.WriteLine("New covid data was saved");
                }
            } catch (Exception err)
            {
                Console.WriteLine(err);
                return;
            }
        }

        private int getCovidLast(int continentId, int locationId)
        {
            try
            {
                using (var context = new CovidDBEntities())
                {
                    var covidsLast = (from covids in context.Covids
                                      where covids.continentId == continentId && covids.locationId == locationId
                                      orderby covids.total_cases
                                      select covids
                                      ).ToList().Last();
                    int totalCases = (int)(covidsLast.total_cases);
                    return totalCases;
                }
            } catch (Exception err)
            {
                return -1;
            }
        }
    }
}
