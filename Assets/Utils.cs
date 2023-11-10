using UnityEngine;

public class Utils {

    static public void Log(object value){

        Debug.Log(value.ToString());
    }

    static public void Log(object[] values){
        string str="";
        foreach (var obj in values)
        {
            str=obj.ToString() + ", ";
        }
        Debug.Log(str);
    }
}