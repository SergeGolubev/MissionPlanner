﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MissionPlanner;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using MissionPlanner.Utilities;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;

namespace MissionPlanner
{
    public class Grid1
    {
        public static MissionPlanner.Plugin.PluginHost Host2;

        const float rad2deg = (float)(180 / Math.PI);
        const float deg2rad = (float)(1.0 / rad2deg);

        const double MinDistanceBetweenLines = 50;

        public struct linelatlng
        {
            // start of line
            public utmpos p1;
            // end of line
            public utmpos p2;
            // used as a base for grid along line (initial setout)
            public utmpos basepnt;
        }

        public enum StartPosition
        {
            Home = 0,
            BottomLeft = 1,
            TopLeft = 2,
            BottomRight = 3,
            TopRight = 4
        }

        static void addtomap(linelatlng pos)
        {
            return;
            //List<PointLatLng> list = new List<PointLatLng>();
            //list.Add(pos.p1.ToLLA());
            //list.Add(pos.p2.ToLLA());

         //   polygons.Routes.Add(new GMapRoute(list, "test") { Stroke = new System.Drawing.Pen(System.Drawing.Color.Yellow,4) });
            
            //.Markers.Add(new GMapMarkerGoogleRed(pnt));

            //map.ZoomAndCenterRoutes("polygons");

           // map.Invalidate();
        }

        static void addtomap(utmpos pos, string tag)
        {
            return;
            //tag = (no++).ToString();
          //  polygons.Markers.Add(new GMapMarkerGoogleRed(pos.ToLLA()));// { ToolTipText = tag, ToolTipMode = MarkerTooltipMode.Always } );

            //map.ZoomAndCenterMarkers("polygons");

            //map.Invalidate();
        }

        public static double cos_aob (utmpos a, utmpos o, utmpos b)
        {
            double A = a.GetDistance(o), B = b.GetDistance(o), C = a.GetDistance(b);

            return (- C * C + A * A + B * B) / (2 * A * B);
        }

        public static List<PointLatLngAlt> CreateGrid(List<PointLatLng> polygon, double altitude, double distance, double spacing, double angle, double turn_radius, StartPosition startpos, bool shutter)
        {
            if (spacing < 10 && spacing != 0)
                spacing = 10;

            if (distance < 5)
                distance = 5;

            if (polygon.Count == 0)
                return new List<PointLatLngAlt>();

            double minLaneSeparationINMeters = 2 * turn_radius;
         /*
            if (minLaneSeparationINMeters < MinDistanceBetweenLines)
            {
                CustomMessageBox.Show("Sorry, minimal distance between lines is " + MinDistanceBetweenLines + " meters");
                minLaneSeparation = (float)(MinDistanceBetweenLines / distance + 1);
                minLaneSeparationINMeters = minLaneSeparation * distance;            
            }
        */

            List<PointLatLngAlt> ans = new List<PointLatLngAlt>();

            int utmzone = ((PointLatLngAlt)polygon[0]).GetUTMZone();

            List<PointLatLngAlt> AltPolygon = new List<PointLatLngAlt>();
            polygon.ForEach(pnt => { AltPolygon.Add(pnt); });

            List<utmpos> utmpositions = utmpos.ToList(PointLatLngAlt.ToUTM(utmzone, AltPolygon), utmzone);

            if (utmpositions[0] != utmpositions[utmpositions.Count - 1])
                utmpositions.Add(utmpositions[0]); // make a full loop

            Rect area = getPolyMinMax(utmpositions);

            // get initial grid

            // used to determine the size of the outer grid area
            double diagdist = area.DiagDistance();

            // somewhere to store out generated lines
            List<linelatlng> grid = new List<linelatlng>();
            // number of lines we need
            int lines = 0;

            // get start point bottom left
            double x = area.MidWidth;
            double y = area.MidHeight;

            addtomap(new utmpos(x, y, utmzone),"Base");

            // get left extent
            double xb1 = x;
            double yb1 = y;
            // to the left
            newpos(ref xb1, ref yb1, angle - 90, diagdist / 2 + distance);
            // backwards
            newpos(ref xb1, ref yb1, angle + 180, diagdist / 2 + distance);

            utmpos left = new utmpos(xb1, yb1, utmzone);

            addtomap(left, "left");

            // get right extent
            double xb2 = x;
            double yb2 = y;
            // to the right
            newpos(ref xb2, ref yb2, angle + 90, diagdist / 2 + distance);
            // backwards
            newpos(ref xb2, ref yb2, angle + 180, diagdist / 2 + distance);

            utmpos right = new utmpos(xb2, yb2, utmzone);

            addtomap(right,"right");

            // set start point to left hand side
            x = xb1;
            y = yb1;

            // draw the outergrid, this is a grid that cover the entire area of the rectangle plus more.
            while (lines < ((diagdist + distance * 2) / distance))
            {
                // copy the start point to generate the end point
                double nx = x;
                double ny = y;
                newpos(ref nx, ref ny, angle, diagdist + distance*2);

                linelatlng line = new linelatlng();
                line.p1 = new utmpos(x, y, utmzone);
                line.p2 = new utmpos(nx, ny, utmzone);
                line.basepnt = new utmpos(x, y, utmzone);
                grid.Add(line);

               // addtomap(line);

                newpos(ref x, ref y, angle + 90, distance);
                lines++;
            }

            // find intersections with our polygon

            // store lines that dont have any intersections
            List<linelatlng> remove = new List<linelatlng>();

            int gridno = grid.Count;

            // cycle through our grid
            for (int a = 0; a < gridno; a++)
            {
                double closestdistance = double.MaxValue;
                double farestdistance = double.MinValue;

                utmpos closestpoint = utmpos.Zero;
                utmpos farestpoint = utmpos.Zero;

                // somewhere to store our intersections
                List<utmpos> matchs = new List<utmpos>();

                int b = -1;
                int crosses = 0;
                utmpos newutmpos = utmpos.Zero;
                foreach (utmpos pnt in utmpositions)
                {
                    b++;
                    if (b == 0)
                    {
                        continue;
                    }
                    newutmpos = FindLineIntersection(utmpositions[b - 1], utmpositions[b], grid[a].p1, grid[a].p2);
                    if (!newutmpos.IsZero)
                    {
                        crosses++;
                        matchs.Add(newutmpos);
                        if (closestdistance > grid[a].p1.GetDistance(newutmpos))
                        {
                            closestpoint.y = newutmpos.y;
                            closestpoint.x = newutmpos.x;
                            closestpoint.zone = newutmpos.zone;
                            closestdistance = grid[a].p1.GetDistance(newutmpos);
                        }
                        if (farestdistance < grid[a].p1.GetDistance(newutmpos))
                        {
                            farestpoint.y = newutmpos.y;
                            farestpoint.x = newutmpos.x;
                            farestpoint.zone = newutmpos.zone;
                            farestdistance = grid[a].p1.GetDistance(newutmpos);
                        }
                    }
                }
                if (crosses == 0) // outside our polygon
                {
                    if (!PointInPolygon(grid[a].p1, utmpositions) && !PointInPolygon(grid[a].p2, utmpositions))
                        remove.Add(grid[a]);
                }
                else if (crosses == 1) // bad - shouldnt happen
                {

                }
                else if (crosses == 2) // simple start and finish
                {
                    linelatlng line = grid[a];
                    line.p1 = closestpoint;
                    line.p2 = farestpoint;
                    grid[a] = line;
                }
                else // multiple intersections
                {
                    linelatlng line = grid[a];
                    remove.Add(line);

                    while (matchs.Count > 1)
                    {
                        linelatlng newline = new linelatlng();

                        closestpoint = findClosestPoint(closestpoint, matchs);
                        newline.p1 = closestpoint;
                        matchs.Remove(closestpoint);

                        closestpoint = findClosestPoint(closestpoint, matchs);
                        newline.p2 = closestpoint;
                        matchs.Remove(closestpoint);

                        newline.basepnt = line.basepnt;

                        grid.Add(newline);
                    }
                }
            }

            foreach (linelatlng line in remove)
            {
                grid.Remove(line);
            }

            foreach (linelatlng line in grid)
            {
                addtomap(line);
            }

            if (grid.Count == 0)
                return ans;

            utmpos startposutm;
         
            switch (startpos)
            {
                default:
                case StartPosition.Home:
                    startposutm = new utmpos(MainV2.comPort.MAV.cs.HomeLocation);
                    break;
                case StartPosition.BottomLeft:
                    startposutm = new utmpos(area.Left, area.Bottom, utmzone);
                    break;
                case StartPosition.BottomRight:
                    startposutm = new utmpos(area.Right, area.Bottom, utmzone);
                    break;
                case StartPosition.TopLeft:
                    startposutm = new utmpos(area.Left, area.Top, utmzone);
                    break;
                case StartPosition.TopRight:
                    startposutm = new utmpos(area.Right, area.Top, utmzone);
                    break;

            }

            //return 
          //      FindPath(grid, startposutm);

            // find closest line point to home
            Tuple <linelatlng, bool> closest_ = findClosestLine(startposutm, grid, 0 /*Lane separation does not apply to starting point*/, angle);
            linelatlng closest = closest_.Item1;
            bool isFurtherMinDintace = closest_.Item2;
            bool oldIsFurtherMinDintace = closest_.Item2; 
            utmpos lastpnt;

            if (closest.p1.GetDistance(startposutm) < closest.p2.GetDistance(startposutm))
            {
                lastpnt = closest.p1;
            }
            else
            {
                lastpnt = closest.p2;
            }

            utmpos newstart = utmpos.Zero, newend = utmpos.Zero, oldstart = newstart, oldend = newend;
            int i = 0;
            bool flightForward = closest.p1.GetDistance(lastpnt) < closest.p2.GetDistance(lastpnt);
            List<PointLatLngAlt> tmp = new List<PointLatLngAlt>();
            List<PointLatLngAlt> oldTmp;

            while (grid.Count > 0)
            {
                oldend = newend;
                oldstart = newstart;
                oldIsFurtherMinDintace = isFurtherMinDintace;
                oldTmp = tmp;
                tmp = new List<PointLatLngAlt>();

                if (flightForward)
                {
                    newstart = newpos(closest.p1, 180 + angle, 2 * turn_radius);
                    addtomap(new utmpos(closest.p1), "M");
                    tmp.Add((new utmpos(closest.p1) { Tag = "M" }));

                    if (spacing > 0)
                    {
                        for (int d = (int)(spacing - ((closest.basepnt.GetDistance(closest.p1)) % spacing));
                            d < (closest.p1.GetDistance(closest.p2));
                            d += (int)spacing)
                        {
                            double ax = closest.p1.x;
                            double ay = closest.p1.y;

                            newpos(ref ax, ref ay, angle, d);
                            addtomap(new utmpos(ax,ay,utmzone),"M");
                            tmp.Add((new utmpos(ax, ay, utmzone) { Tag = "M"}));
                          //ans.Add((new utmpos(ax, ay, utmzone) { Tag = "M" }));

                          //  if (shutter.ToLower().StartsWith("y"))
                              //  AddDigicamControlPhoto();
                        }
                    }


                    newend = newpos(closest.p2, angle, 2 * turn_radius);
                    addtomap(new utmpos(closest.p2), "M");
                    tmp.Add((new utmpos(closest.p2) { Tag = "M" }));

                    lastpnt = closest.p2;

                    grid.Remove(closest);
                    if (grid.Count != 0) { 
                        closest_ = findClosestLine(newend, grid, minLaneSeparationINMeters, angle); 
                        closest = closest_.Item1;
                        isFurtherMinDintace = closest_.Item2;
                    }
                }
                else
                {
                    newstart = newpos(closest.p2, 180 + angle, -2 * turn_radius);
                    addtomap(new utmpos(closest.p2), "M");
                    tmp.Add((new utmpos(closest.p2) { Tag = "M" }));
                    
                    if (spacing > 0)
                    {
                        for (int d = (int)((closest.basepnt.GetDistance(closest.p2)) % spacing);
                            d < (closest.p1.GetDistance(closest.p2));
                            d += (int)spacing)
                        {
                            double ax = closest.p2.x;
                            double ay = closest.p2.y;

                            newpos(ref ax, ref ay, angle, -d);
                            addtomap(new utmpos(ax, ay, utmzone), "M");
                            tmp.Add((new utmpos(ax, ay, utmzone) { Tag = "M" }));
                            //ans.Add((new utmpos(ax, ay, utmzone) { Tag = "M" }));

                           // if (shutter.ToLower().StartsWith("y"))
                            //    AddDigicamControlPhoto();
                        }
                    }

                    newend = newpos(closest.p1, angle, -2 * turn_radius);
                    addtomap(new utmpos(closest.p1), "M");
                    tmp.Add((new utmpos(closest.p1) { Tag = "M" }));
                 
                    lastpnt = closest.p1;

                    grid.Remove(closest);
                    if (grid.Count != 0)
                    {
                        closest_ = findClosestLine(newend, grid, minLaneSeparationINMeters, angle);
                        closest = closest_.Item1;
                        isFurtherMinDintace = closest_.Item2;
                    }
                }

                if (i != 0)
                {
                    
                    int sign = flightForward ? -1 : 1;
                    double addDist = oldend.GetDistance(newstart) * cos_aob(oldend, newstart, newend);

                    if (addDist < 0)
                    {
                        newstart = newpos(newstart, 180 + angle, sign * addDist);
                    }
                    else
                    {
                        oldend = newpos(oldend, angle, sign * addDist);
                    }

                    
                    addtomap(oldstart, "S");
                    ans.Add(oldstart);
                    for (int j = 0; j < oldTmp.Count; j++)
                        ans.Add(oldTmp[j]);
                    addtomap(oldend, "E");
                    ans.Add(oldend);
                    oldTmp.Clear();

                    //flying around in a circle
                    if (!oldIsFurtherMinDintace)
                    {
                        double tmpAngle = angle;
                        utmpos midpos = newpos(oldend, 90 + tmpAngle, minLaneSeparationINMeters);
                        
                        if (midpos.GetDistance(oldend) < midpos.GetDistance(newstart) &&
                            midpos.GetDistance(newstart) > newstart.GetDistance(oldend))
                        {
                        }
                        else
                        {
                            tmpAngle += 180;
                        }
                       
                        midpos = newpos(oldend, 90 + tmpAngle, minLaneSeparationINMeters);                      
                        ans.Add(midpos);
                        midpos = newpos(midpos, angle, sign * minLaneSeparationINMeters);
                        ans.Add(midpos);
                        midpos = newpos(midpos, 270 + tmpAngle, minLaneSeparationINMeters);
                        ans.Add(midpos);
                    }
                } else {
                    double cos_phi = cos_aob(startposutm, newstart, newend);
                    if (cos_phi > 0)
                    {
                        if (Math.Pow(startposutm.GetDistance(newstart), 2) * (1 - cos_phi * cos_phi) > 4 * Math.Pow(minLaneSeparationINMeters, 2))
                        {
                            utmpos midpos = newpos(newstart, 90 + angle, minLaneSeparationINMeters);
                            if (midpos.GetDistance(startposutm) > startposutm.GetDistance(newstart))
                            {
                                midpos = newpos(newstart, 270 + angle, minLaneSeparationINMeters);
                            }
                            ans.Add(midpos);
                        }
                        else
                        {
                            utmpos midpos = newpos(newstart, 90 + angle, minLaneSeparationINMeters);
                            double tmpAngle = angle;
                            if (midpos.GetDistance(startposutm) < startposutm.GetDistance(newstart))
                            {
                                tmpAngle += 180;
                            }
                            int sign = flightForward ? -1 : 1;
                            midpos = newpos(newstart, 90 + tmpAngle, minLaneSeparationINMeters);
                            ans.Add(midpos);
                            midpos = newpos(midpos, angle, sign * minLaneSeparationINMeters);
                            ans.Add(midpos);
                            midpos = newpos(midpos, 270 + tmpAngle, minLaneSeparationINMeters);
                            ans.Add(midpos);
                        }
                    }

                }
                flightForward = !flightForward;
                i++;
            }    

            addtomap(newstart, "S");
            ans.Add(newstart);
            for (int j = 0; j < tmp.Count; j++)
                ans.Add(tmp[j]);
            tmp.Clear();
            addtomap(newend, "E");
            ans.Add(newend);

                

            // set the altitude on all points
            ans.ForEach(plla => { plla.Alt = altitude; });

            return ans;
        }

        //http://en.wikipedia.org/wiki/Rapidly_exploring_random_tree



        // a*
        static List<PointLatLngAlt> FindPath(List<linelatlng> grid1, utmpos startposutm)
        {
            List<PointLatLngAlt> answer = new List<PointLatLngAlt>();

            List<linelatlng> closedset = new List<linelatlng>();
            List<linelatlng> openset = new List<linelatlng>(); // nodes to be travered
            Hashtable came_from = new Hashtable();
            List<linelatlng> grid = new List<linelatlng>();


            linelatlng start = new linelatlng() { p1 = startposutm, p2 = startposutm };

            grid.Add(start);
            grid.AddRange(grid1);
            openset.Add(start);

            Hashtable g_score = new Hashtable();
            Hashtable f_score = new Hashtable();
            g_score[start] = 0.0;
            f_score[start] = (double)g_score[start] + heuristic_cost_estimate(grid,0,start); // heuristic_cost_estimate(start, goal)

            linelatlng current = start;

            while (openset.Count > 0)
            {
                current = FindLowestFscore(g_score, openset); // lowest f_score
                openset.Remove(current);
                closedset.Add(current);
                foreach (var neighbor in neighbor_nodes(current, grid))
                {
                    double tentative_g_score = (double)g_score[current];

                    double dist1 = current.p1.GetDistance(neighbor.p1);
                    double dist2 = current.p1.GetDistance(neighbor.p2);
                    double dist3 = current.p2.GetDistance(neighbor.p1);
                    double dist4 = current.p2.GetDistance(neighbor.p2);

                    tentative_g_score += (dist1 + dist2 + dist3 + dist4) / 4;

                    tentative_g_score  += neighbor.p1.GetDistance(neighbor.p2);

                    //tentative_g_score += Math.Min(Math.Min(dist1, dist2), Math.Min(dist3, dist4));
                    //tentative_g_score += Math.Max(Math.Max(dist1, dist2), Math.Max(dist3, dist4));

                   // if (closedset.Contains(neighbor) && tentative_g_score >= (double)g_score[neighbor])
                   //     continue;

                    if (!closedset.Contains(neighbor) ||
                       tentative_g_score < (double)g_score[neighbor])
                    {
                        came_from[neighbor] = current;
                        g_score[neighbor] = tentative_g_score;
                        f_score[neighbor] = tentative_g_score + heuristic_cost_estimate(grid, tentative_g_score, neighbor);
                        Console.WriteLine("neighbor score: " + g_score[neighbor] + " " + f_score[neighbor]);
                        if (!openset.Contains(neighbor))
                            openset.Add(neighbor);
                    }
                }
               
            }

            // bad
            //linelatlng ans = FindLowestFscore(g_score, grid);

          //  foreach (var ans in grid)
            {
                List<linelatlng> list = reconstruct_path(came_from, current);
              //  list.Insert(0,current);
                //list.Remove(start);
                //list.Remove(start);
                Console.WriteLine("List " + list.Count + " " + g_score[current]);
               {
                   List<utmpos> temp = new List<utmpos>();
                   temp.Add(list[0].p1);
                   temp.Add(list[0].p2);
                   utmpos oldpos = findClosestPoint(startposutm, temp);

                   foreach (var item in list) 
                   {
                       double dist1 = oldpos.GetDistance(item.p1);
                       double dist2 = oldpos.GetDistance(item.p2);
                       if (dist1 < dist2)
                       {
                           answer.Add(new PointLatLngAlt(item.p1));
                           answer.Add(new PointLatLngAlt(item.p2));
                           oldpos = item.p2;
                       }
                       else
                       {
                           answer.Add(new PointLatLngAlt(item.p2));
                           answer.Add(new PointLatLngAlt(item.p1));
                           oldpos = item.p1;
                       }
                   }
                   //return answer;
               }
            }

            List<PointLatLng> list2 = new List<PointLatLng>();

            answer.ForEach(x => { list2.Add(x); });

            GMapPolygon wppoly = new GMapPolygon(list2, "Grid");

            Console.WriteLine("dist " + (wppoly.Distance));

            return answer;
        }

        static double heuristic_cost_estimate(List<linelatlng> grid, double sofar, linelatlng current_node)
        {
            double ans = 0;

            linelatlng lastx = grid[0];

            grid.ForEach(x => 
            { 
                ans += x.p1.GetDistance(x.p2);
                ans += x.p1.GetDistance(lastx.p1);
                lastx = x; 
            });



            return ans - sofar * 0.95;
        }

        static List<linelatlng> reconstruct_path(Hashtable came_from, linelatlng current_node)
        {
            List<linelatlng> ans = new List<linelatlng>();
            if (came_from.ContainsKey(current_node))
            {
                
                ans.AddRange(reconstruct_path(came_from, (linelatlng)came_from[current_node]));
                ans.Add((linelatlng)came_from[current_node]);
                return ans;
            }
            else
            {
                ans.Add(current_node);
                return ans;
            }
        }


        static private List<linelatlng> neighbor_nodes(linelatlng current, List<linelatlng> grid)
        {
            List<linelatlng> neighbors = new List<linelatlng>();

            foreach (var item in grid)
            {
               // if (item.Equals(current))
              //      continue;

                neighbors.Add(item);
            }

            return neighbors;
        }

        static private linelatlng FindLowestFscore(Hashtable f_score, List<linelatlng> openset)
        {
            linelatlng lowest = openset[0];
            int lowestint = int.MaxValue;

            foreach (linelatlng key in openset)
            {
                if (f_score.ContainsKey(key) && (double)f_score[key] < lowestint)
                {
                    lowestint = (int)(double)f_score[key];
                    lowest = key;
                }
            }

            Console.WriteLine("Lowest " + lowestint);

            return lowest;
        }


        static Rect getPolyMinMax(List<utmpos> utmpos)
        {
            if (utmpos.Count == 0)
                return new Rect();

            double minx, miny, maxx, maxy;

            minx = maxx = utmpos[0].x;
            miny = maxy = utmpos[0].y;

            foreach (utmpos pnt in utmpos)
            {
                minx = Math.Min(minx, pnt.x);
                maxx = Math.Max(maxx, pnt.x);

                miny = Math.Min(miny, pnt.y);
                maxy = Math.Max(maxy, pnt.y);
            }

            return new Rect(minx, maxy, maxx - minx,miny - maxy);
        }

        // polar to rectangular
        static void newpos(ref double x, ref double y, double bearing, double distance)
        {
            double degN = 90 - bearing;
            if (degN < 0)
                degN += 360;
            x = x + distance * Math.Cos(degN * deg2rad);
            y = y + distance * Math.Sin(degN * deg2rad);
        }

        // polar to rectangular
        static utmpos newpos(utmpos input, double bearing, double distance)
        {
            double degN = 90 - bearing;
            if (degN < 0)
                degN += 360;
            double x = input.x + distance * Math.Cos(degN * deg2rad);
            double y = input.y + distance * Math.Sin(degN * deg2rad);

            return new utmpos(x, y, input.zone);
        }

        /// <summary>
        /// from http://stackoverflow.com/questions/1119451/how-to-tell-if-a-line-intersects-a-polygon-in-c
        /// </summary>
        /// <param name="start1"></param>
        /// <param name="end1"></param>
        /// <param name="start2"></param>
        /// <param name="end2"></param>
        /// <returns></returns>
        public static utmpos FindLineIntersection(utmpos start1, utmpos end1, utmpos start2, utmpos end2)
        {
            double denom = ((end1.x - start1.x) * (end2.y - start2.y)) - ((end1.y - start1.y) * (end2.x - start2.x));
            //  AB & CD are parallel         
            if (denom == 0)
                return utmpos.Zero;
            double numer = ((start1.y - start2.y) * (end2.x - start2.x)) - ((start1.x - start2.x) * (end2.y - start2.y));
            double r = numer / denom;
            double numer2 = ((start1.y - start2.y) * (end1.x - start1.x)) - ((start1.x - start2.x) * (end1.y - start1.y));
            double s = numer2 / denom;
            if ((r < 0 || r > 1) || (s < 0 || s > 1))
                return utmpos.Zero;
            // Find intersection point      
            utmpos result = new utmpos();
            result.x = start1.x + (r * (end1.x - start1.x));
            result.y = start1.y + (r * (end1.y - start1.y));
            result.zone = start1.zone;
            return result;
        }

        static utmpos findClosestPoint(utmpos start, List<utmpos> list)
        {
            utmpos answer = utmpos.Zero;
            double currentbest = double.MaxValue;

            foreach (utmpos pnt in list)
            {
                double dist1 = start.GetDistance(pnt);

                if (dist1 < currentbest)
                {
                    answer = pnt;
                    currentbest = dist1;
                }
            }

            return answer;
        }

        // Add an angle while normalizing output in the range 0...360
        static double AddAngle(double angle, double degrees)
        {
            angle += degrees;

            angle = angle % 360;

            if (angle < 0)
            {
                angle += 360;
            }
            return angle;
        }

        static Tuple<linelatlng, bool> findClosestLine(utmpos start, List<linelatlng> list, double minDistance, double angle)
        {
            // By now, just add 5.000 km to our lines so they are long enough to allow intersection
            double METERS_TO_EXTEND = 5000000;
            bool IsFutherMinDistance = true;
            double perperndicularOrientation = AddAngle(angle, 90);

            // Calculation of a perpendicular line to the grid lines containing the "start" point
            /*
             *  --------------------------------------|------------------------------------------
             *  --------------------------------------|------------------------------------------
             *  -------------------------------------start---------------------------------------
             *  --------------------------------------|------------------------------------------
             *  --------------------------------------|------------------------------------------
             *  --------------------------------------|------------------------------------------
             *  --------------------------------------|------------------------------------------
             *  --------------------------------------|------------------------------------------
             */
            utmpos start_perpendicular_line = newpos(start, perperndicularOrientation, -METERS_TO_EXTEND);
            utmpos stop_perpendicular_line = newpos(start, perperndicularOrientation, METERS_TO_EXTEND);

            // Store one intersection point per grid line
            Dictionary<utmpos, linelatlng> intersectedPoints = new Dictionary<utmpos, linelatlng>();
            // lets order distances from every intersected point per line with the "start" point
            Dictionary<double, utmpos> ordered_min_to_max = new Dictionary<double, utmpos>();

            foreach (linelatlng line in list)
            {
                // Extend line at both ends so it intersecs for sure with our perpendicular line
                utmpos extended_line_start = newpos(line.p1, angle, -METERS_TO_EXTEND);
                utmpos extended_line_stop = newpos(line.p2, angle, METERS_TO_EXTEND);
                // Calculate intersection point
                utmpos p = FindLineIntersection(extended_line_start, extended_line_stop, start_perpendicular_line, stop_perpendicular_line);
                
                // Store it
                intersectedPoints[p] = line;

                // Calculate distances between interesected point and "start" (i.e. line and start)
                double distance_p = start.GetDistance(p);
                if (!ordered_min_to_max.ContainsKey(distance_p))
                    ordered_min_to_max.Add(distance_p, p);
            }
     
            // Acquire keys and sort them.
            List<double> ordered_keys = ordered_min_to_max.Keys.ToList();
            ordered_keys.Sort();

            // Lets select a line that is the closest to "start" point but "mindistance" away at least.
            // If we have only one line, return that line whatever the minDistance says
            double key = double.MaxValue;
            int i = 0;
            while (key == double.MaxValue && i < ordered_keys.Count)
            {
                if (ordered_keys[i] >= minDistance)
                    key = ordered_keys[i];
                i++;
            }

            // If no line is selected (because all of them are closer than minDistance, then get the farest one
            if (key == double.MaxValue)
            {
                key = ordered_keys[ordered_keys.Count - 1];
                IsFutherMinDistance = false;
            }

            // return line
            return Tuple.Create(intersectedPoints[ordered_min_to_max[key]], IsFutherMinDistance);

        }

        static bool PointInPolygon(utmpos p, List<utmpos> poly)
        {
            utmpos p1, p2;
            bool inside = false;

            if (poly.Count < 3)
            {
                return inside;
            }
            utmpos oldPoint = new utmpos(poly[poly.Count - 1]);

            for (int i = 0; i < poly.Count; i++)
            {

                utmpos newPoint = new utmpos(poly[i]);

                if (newPoint.y > oldPoint.y)
                {
                    p1 = oldPoint;
                    p2 = newPoint;
                }
                else
                {
                    p1 = newPoint;
                    p2 = oldPoint;
                }

                if ((newPoint.y < p.y) == (p.y <= oldPoint.y)
                    && ((double)p.x - (double)p1.x) * (double)(p2.y - p1.y)
                    < ((double)p2.x - (double)p1.x) * (double)(p.y - p1.y))
                {
                    inside = !inside;
                }
                oldPoint = newPoint;
            }
            return inside;
        }


    }
}