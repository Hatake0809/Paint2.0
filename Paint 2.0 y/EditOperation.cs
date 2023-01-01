using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint_2._0
{
    internal class EditOperation
    {
        private UndoRedoClass data;
        private bool txtAreaTextChangeRequired = true;
        public EditOperation()
        {
            data = new UndoRedoClass();
        }

        public bool TxtAreaTextChangeRequired
        {
            get
            {
                return txtAreaTextChangeRequired;
            }

            set
            {
                txtAreaTextChangeRequired = value;
            }
        }
        public Bitmap UndoClicked()
        {
            TxtAreaTextChangeRequired = false;
            return data.Undo();
        }

        public Bitmap RedoClicked()
        {
            TxtAreaTextChangeRequired = false;
            return data.Redo();
        }

        public void Add_UndoRedo(Bitmap item)
        {
            data.Addİtem(item);
        }

        public bool CanUndo()
        {
            return data.CanUndo();
        }

        public bool CanRedo()
        {
            return data.CanRedo();
        }
    }
}
