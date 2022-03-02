using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomTag
{
    internal class TagsUtils
    {
        public static List<RoomTagType> GetRoomTagTypes(Document doc)
        {
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(doc);
            filteredElementCollector.OfClass(typeof(FamilySymbol));
            filteredElementCollector.OfCategory(BuiltInCategory.OST_RoomTags);
            var roomTagTypes = filteredElementCollector.Cast<RoomTagType>().ToList();
            return roomTagTypes;
        }


        public static List<Room> GetRooms(Document doc)
        {
            List<Room> rooms = new FilteredElementCollector(doc)
                .OfClass(typeof(SpatialElement))
                .OfType<Room>()
                .Where(x => x.Area>0)
                .ToList();
            return rooms;
        }

        public static XYZ GetElementCenter(Element element)
        {
            BoundingBoxXYZ bounding = element.get_BoundingBox(null);
            return (bounding.Max + bounding.Min) / 2;
        }
    }
}
