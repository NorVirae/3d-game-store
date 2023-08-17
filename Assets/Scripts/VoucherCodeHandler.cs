using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class VoucherCodeHandler : MonoBehaviour
{

    [SerializeField]
    TextField voucherInput;

    public struct VoucherModel
    {
        public string VoucherCode;
        public int ItemAmount;
        public string ItemId;
        public string CreationDate;
    }

    void Start()
    {

    }

    public void RetrieveVoucherCode()
    {
        string voucherCode = voucherInput.text;


    }

    void findVoucherItem(string voicherCode)
    {

    }
}


