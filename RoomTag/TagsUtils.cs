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
        public static List<FamilySymbol> GetRoomTagTypes(ExternalCommandData commandData)
        {
            
            Document doc = commandData.Application.ActiveUIDocument.Document;

            var roomTagTypes = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_RoomTags)
                .OfClass(typeof(FamilySymbol))
                .Cast<FamilySymbol>()
                .ToList();
            return roomTagTypes;

        }


        public static List<Room> GetRooms(ExternalCommandData commandData)
        {

            Document doc = commandData.Application.ActiveUIDocument.Document;

            var rooms = new FilteredElementCollector(doc)
                .OfClass(typeof(Room))
                .OfType<Room>()
                
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
