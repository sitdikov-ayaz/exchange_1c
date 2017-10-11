using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace ExchangeVS
{
    class KillSessions
    {
        //thread = new Thread(this.func);
        //thread.IsBackground = true;
        //thread.Start(Val);
   }
}



//class myThreadDownloadImagesAll
//{
//    Thread thread;

//    public myThreadDownloadImagesAll(string[] Val)
//    {

//        thread = new Thread(this.func);
//        thread.IsBackground = true;
//        thread.Start(Val);
//    }

//    void func(object Val)//Функция потока, передаем параметр
//    {
//        string[] ImgCode = (string[])Val;
//        string[] files = new[] { ".png", ".jpg", ".bmp" };
//        int filesCount = files.Count();

//        foreach (string imgCode in ImgCode)
//        {
//         }

//    }


//}



//CoInitializeEx(nil, COINIT_MULTITHREADED);

//ErrMsg1C:= 'Подключение к агенту через 1с ...';
//  Synchronize(AddLog);

//  if NOT ConnectTo1C() then begin Synchronize(AddLog); exit; end;


//  TRY
//  V8APPt.ПроцедурыОбменаДаннымиДоп.ОтключитьСеансыБазы(pBaseClaster, pBaseName, pBaseLogin, pBasePassword,0);
//ErrMsg1C:='Сансы закрыты.';
//  Synchronize(AddLog);
//except
// on E: Exception do
//   begin
//      ErrMsg1C:='Ошибка при закрыти сенсов. '+E.Message;
//      Synchronize(AddLog);
//end;
//  end;

//  UnitModule.MyUnAssigned;

//  CoUninitialize;