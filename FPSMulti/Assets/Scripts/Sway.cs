using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPSMulti
{
    public class Sway : MonoBehaviour
    {
        #region Variables
        public float intensity; //jak bardzo sie pochyla
        public float smooth; //jak szybko wraca do pionu

        private Quaternion targetRotation;
        private Quaternion originRotation;
        #endregion



        #region Monobehaviour Callbacks

        private void Start()
        {
            originRotation = transform.localRotation;
        }
        private void Update()
        {
            UpdateSway();
        }
        #endregion



        #region Private Methods
        private void UpdateSway()
        {
            //controls
            float xmouse = Input.GetAxis("Mouse X");
            float ymouse = Input.GetAxis("Mouse Y");

            //calculate target rotation
            Quaternion xadj = Quaternion.AngleAxis(-intensity*xmouse, Vector3.up);
            Quaternion yadj = Quaternion.AngleAxis(intensity*ymouse, Vector3.right);
            Quaternion targetRotation = originRotation * xadj*yadj;

            //rotate towards target rotation
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * smooth);
        }
        #endregion
    }
}