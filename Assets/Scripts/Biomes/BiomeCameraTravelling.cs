using System.Collections;
using UnityEngine;

public class BiomeCameraTravelling : MonoBehaviour
{
    public Camera mainCamera;
    public Transform fireBiomeFocus;
    public Transform thunderBiomeFocus;
    public Transform iceBiomeFocus;
    public Transform playerCameraTarget;
    public float travelDuration = 3f;

    void Start()
    {
        GameEventsManager.instance.biomeEvents.onFireBiomeEnter += FireBiomeTravelling;
        GameEventsManager.instance.biomeEvents.onThunderBiomeEnter += ThunderBiomeTravelling;
        GameEventsManager.instance.biomeEvents.onIceBiomeEnter += IceBiomeTravelling;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.biomeEvents.onFireBiomeEnter -= FireBiomeTravelling;
        GameEventsManager.instance.biomeEvents.onThunderBiomeEnter -= ThunderBiomeTravelling;
        GameEventsManager.instance.biomeEvents.onIceBiomeEnter -= IceBiomeTravelling;
    }

    /// <summary>
    /// Init every parameters for the travelling
    /// </summary>
    public void InitializeBiomes()
    {
        GameObject fireBiome = GameObject.Find("FireBiome(Clone)");
        GameObject iceBiome = GameObject.Find("IceBiome(Clone)");
        GameObject thunderBiome = GameObject.Find("ThunderBiome(Clone)");

        if (fireBiome != null) { fireBiomeFocus = fireBiome.transform; }
        if (iceBiome != null) { iceBiomeFocus = iceBiome.transform; }
        if (thunderBiome != null) { thunderBiomeFocus = thunderBiome.transform; }
    }

    private void FireBiomeTravelling(string _useless)
    {
        StartCoroutine(DoCameraTravel(fireBiomeFocus));
    }

    private void ThunderBiomeTravelling(string _useless)
    {
        StartCoroutine(DoCameraTravel(thunderBiomeFocus));
    }

    private void IceBiomeTravelling(string _useless)
    {
        StartCoroutine(DoCameraTravel(iceBiomeFocus));
    }

    /// <summary>
    /// Do the camera travel. Basically will take the camera and put it in the top of the world based 
    /// on a gameobject create by the biome generation
    /// </summary>
    /// <param name="biomeTransform"></param>
    /// <returns></returns>
    private IEnumerator DoCameraTravel(Transform biomeTransform)
    {
        GameEventsManager.instance.loreEvents.OnImportantLoreEvent();

        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;

        Vector3 topViewPos = biomeTransform.position + Vector3.up * 20f;
        Quaternion topViewRot = Quaternion.Euler(90f, 0f, 0f);

        float elapsed = 0f;

        while (elapsed < travelDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / travelDuration;

            mainCamera.transform.position = Vector3.Lerp(startPos, topViewPos, t);
            mainCamera.transform.rotation = Quaternion.Slerp(startRot, topViewRot, t);

            yield return null;
        }

        mainCamera.transform.position = topViewPos;
        mainCamera.transform.rotation = Quaternion.LookRotation(biomeTransform.position - topViewPos);

        yield return new WaitForSeconds(2f);

        Vector3 pos = mainCamera.transform.position;

        pos.y = 0.585f;
        mainCamera.transform.position = pos;
        mainCamera.transform.rotation = playerCameraTarget.rotation;

        GameEventsManager.instance.loreEvents.OnImportantLoreEvent();
    }
}
