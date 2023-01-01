using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint_2._0
{
    public class UndoRedoClass
    {
        private Stack<Bitmap> UndoStack;
        private Stack<Bitmap> RedoStack;
        public UndoRedoClass()
        {
            UndoStack = new Stack<Bitmap>();
            RedoStack = new Stack<Bitmap>();
        }
        public void Clear()
        {
            UndoStack.Clear();
            RedoStack.Clear();
        }
        public void Addİtem(Bitmap item) 
        {
            UndoStack.Push(item);//stacklememize yarıyor en üste ekliyor 
        }
        public Bitmap Undo()
        {
            Bitmap item= UndoStack.Pop();//.pop En üstteki elemanı siler ve geri almayı sağlar
            RedoStack.Push(item);//geri almayı düşünürse kullanıcı böylece daha önce pop metodu ile sildiğimizi bu sefer iler almaya stackliyoruz
            return UndoStack.First();//listenin ilk elemanını seçip döndürür
        }
        public Bitmap Redo()
        {
            if(RedoStack.Count == 0) 
                return UndoStack.First(); //eğer ileri almak için stack yoksa en son geri aldığımız stack'ı döndürüyor 
            Bitmap item= RedoStack.Pop();//ileri alırsan stack'ı siliyor
            return UndoStack.First() ;//ileri alırsan son geri aldığın stack'ı dönürüyor 
        }
        public bool CanUndo() 
        {
            return UndoStack.Count > 1;// eğer geri alabiliyorsa ondan bir önceki stack'ı döndürsün 
        }        
        public bool CanRedo()
        {
            return RedoStack.Count > 0;
        }
        public List<Bitmap> UndoItems()
        {
            return UndoStack.ToList();//geri alınanları list şeklinde döndürüyor
        }

        public List<Bitmap> RedoItems()
        {
            return RedoStack.ToList();//iler, alınanları list şeklinde döndürüyor
        }

    }
}
