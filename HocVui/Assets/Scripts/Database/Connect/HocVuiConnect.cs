using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HocVuiConnect
{
    static string Server = "139.180.190.203,1433";
    static string id = "sa";
    static string password = "Uyw$l9*L1D@yx#Gt";
    static string database = "Hocvui";

    public static string GetConnectHocVui()
    {
        return $"Server = {Server}; uid = {id}; pwd = {password}; Database = {database};";
    }

}
