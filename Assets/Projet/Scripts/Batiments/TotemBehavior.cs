using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemBehavior : MonoBehaviour
{
    //script du totem
    //ajouter une verif au S2 si le joueur a déja l'unité ajoutée

    [SerializeField] private int rosterUnit;
    [SerializeField] private float timeToCollect;
    [SerializeField] private float rangeCollection;

    private float count;
    private bool activated = true;

    FMOD.Studio.EventInstance totemIdle;
    string totemActivate = "event:/Building/Build_Temple/Build_Templ_Activation/Build_Templ_Activation";


    [SerializeField] private GameObject vfxTotem;

    private void Start()
    {
        totemIdle = FMODUnity.RuntimeManager.CreateInstance("event:/Building/Build_Temple/Build_Templ_Idle/Build_Templ_Idle");
        totemIdle.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
        totemIdle.start();

        vfxTotem.SetActive(false);
    }

    void Update()
    {
        if (!activated)
            return;

        if (Vector3.Distance(HQBehavior.instance.gameObject.transform.position, transform.position) < rangeCollection)
        {
            count += Time.deltaTime;
            if (!vfxTotem.activeInHierarchy) vfxTotem.SetActive(true);
            if (count > timeToCollect)
            {
                HQBehavior.instance.AddToRoaster(rosterUnit);
                activated = false;
                FMODUnity.RuntimeManager.PlayOneShot(totemActivate);
                vfxTotem.SetActive(false);
                NewSelectionManager.instance.onChangeSelection();
            }
        }
        else
        {
            count = 0;
            if (vfxTotem.activeInHierarchy) vfxTotem.SetActive(false);
        }
    }
}
