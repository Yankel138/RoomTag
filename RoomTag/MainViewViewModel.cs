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

        //public Pipe Pipe { get; }
        public List<FamilySymbol> Tags { get; }
        public DelegateCommand SaveCommand { get; }
        public FamilySymbol SelectedTagType { get; set; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            Pipe = SelectionUtils.GetObject<Pipe>(commandData, "Выберите трубу");
            Tags = TagsUtils.GetRoomTagTypes(commandData);
            SaveCommand = new DelegateCommand(OnSaveCommand);

        }

        private void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<Room> rooms = TagsUtils.GetRooms(_commandData);
            var pipeCurve = pipeLocCurve.Curve;
            var pipeMidPoint = (pipeCurve.GetEndPoint(0) + pipeCurve.GetEndPoint(1)) / 2;

            using (var ts = new Transaction(doc, "Create tag"))
            {
                ts.Start();
                IndependentTag.Create(doc, SelectedTagType.Id, doc.ActiveView.Id, new Reference(Pipe), false, TagOrientation.Horizontal, pipeMidPoint);
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