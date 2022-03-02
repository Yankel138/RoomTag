using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RoomTag
{
    internal class MainViewViewModel
    {
        private ExternalCommandData _commandData;
        private Document _doc;
        public List<RoomTagType> Tags { get; }
        public DelegateCommand SaveCommand { get; }
        public RoomTagType SelectedTagType { get; set; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            _doc = commandData.Application.ActiveUIDocument.Document;
            Tags = TagsUtils.GetRoomTagTypes(_doc);
            SaveCommand = new DelegateCommand(OnSaveCommand);

        }

        private void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<Room> rooms = TagsUtils.GetRooms(_doc);
            View view = _doc.ActiveView;

                using (var ts = new Transaction(doc, "Create tag"))
                {
                    ts.Start();
                    foreach (var room in rooms)
                    {
                        XYZ roomCenter = TagsUtils.GetElementCenter(room);
                        UV point = new UV(roomCenter.X, roomCenter.Y);
                        Autodesk.Revit.DB.Architecture.RoomTag newTag = doc.Create.NewRoomTag(new LinkElementId(room.Id), point, null);
                        newTag.RoomTagType = SelectedTagType;
                    }
                    ts.Commit();
                }
            RaiseCloseRequest();
        }

        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}