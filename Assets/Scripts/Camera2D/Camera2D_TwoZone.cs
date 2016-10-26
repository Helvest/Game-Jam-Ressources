using UnityEngine;
using System.Collections;

//limite le deplacement de la cible dans 2 carrés
public class Camera2D_TwoZone : Camera2DLogic
{
	[SerializeField]
	private bool isRight = false;
	[SerializeField]
	private bool isDown = false;
	[SerializeField]
	private float CameraSpeed = 1.0F;
	[SerializeField]
	private float distanceMin = 3.0F;
	[SerializeField]
	private float distanceFree = 2.0F;

	private float distanceMax = 0;
    private float objectifX = 0;
	private float objectifY = 0;
	private sbyte DirectionX = 1;
    private sbyte DirectionY = 1;
    private float currentVelocity;
	private float lastDistanceX;
	private float lastDistanceY;
	private bool resetX = false;
	private bool resetY = false;
	private float waitX = 0;
	private float waitY = 0;
	private float antiObjectifX;
	private float antiObjectifY;

	void Start()
    {
		if (isRight)
            DirectionX = -1;
        if (isDown)
            DirectionY = -1;

        distanceMax = distanceMin + distanceFree;
    }

	public override void UpdatePoint(ref Point2D point2D)
    {
        if (plan == EnumCameraPlan.X || plan == EnumCameraPlan.XY)
        {
			point2D.position.x = Calcul(_transform.position.x, point2D.position.x, ref DirectionX, ref lastDistanceX, ref resetX, ref objectifX, ref waitX, ref antiObjectifX);
		}
		if (plan == EnumCameraPlan.Y || plan == EnumCameraPlan.XY)
        {
			point2D.position.y = Calcul(_transform.position.y, point2D.position.y, ref DirectionY, ref lastDistanceY, ref resetY, ref objectifY, ref waitY, ref antiObjectifY);
        }
	}

    private float Calcul(float transPosition, float point, ref sbyte direction, ref float lastDistance, ref bool reset, ref float objectif, ref float wait, ref float antiObjectif)
    {
		distanceMax = distanceMin + distanceFree;


		if (reset)
		{
			//si intanstaner
			if(CameraSpeed == 0)
			{
				reset = false;
				return objectif;
			}
			//update transition
			wait += Time.deltaTime / CameraSpeed;
			if (wait >= 1) {
				wait = 0;
				reset = false;
				return objectif;
			}

			float ecart = transPosition - lastDistance;

			//si le joueur continue à avancé mise à jour du point
			if (ecart * direction > 0) { 
				//objectif = distanceMin * direction + transPosition;
				lastDistance = transPosition;
				objectif += ecart;
				antiObjectif += ecart;
				return Mathf.Lerp(antiObjectif, objectif, wait);
			}

			//Distance cible-objectif
			float distanceActuelleO = (objectif - transPosition) * direction;

			//si trop proche des bords
			if (distanceActuelleO > distanceMax)
			{
				direction = (sbyte)-direction;
				objectif = distanceMax * direction + transPosition;
				antiObjectif = point;
				wait = 0;
				lastDistance = transPosition + distanceFree * direction;
				return Mathf.Lerp(antiObjectif, objectif, wait);
			}

			return Mathf.Lerp(antiObjectif, objectif, wait);
		}

		//Distance cible-camera
		float distanceActuelle = (point - transPosition) * direction;

		//si trop proche du centre
		if (distanceActuelle < distanceMin)
		{
			objectif = distanceMin * direction + transPosition;
			return objectif;
		}
		//si trop proche des bords
		else if (distanceActuelle > distanceMax)
		{
			reset = true;
			direction = (sbyte)-direction;
			objectif = distanceMin * direction + transPosition;
			antiObjectif = point;
			lastDistance = transPosition;
		}

		return point;
	}

#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Vector2 gizPos = transform.position;

		if (plan == EnumCameraPlan.X || plan == EnumCameraPlan.XY)
		{
			Debug.DrawLine(
				new Vector2(objectifX - distanceMin * DirectionX, transform.position.y),
				new Vector2(objectifX - distanceMax * DirectionX, transform.position.y),
				Color.green
			);
			Debug.DrawLine(
				new Vector2(objectifX + distanceMin * DirectionX, transform.position.y),
				new Vector2(objectifX + distanceMax * DirectionX, transform.position.y),
				Color.red
			);
			gizPos.x = objectifX;
		}
			else
		if (plan == EnumCameraPlan.Y || plan == EnumCameraPlan.XY)
		{
			Debug.DrawLine(
				new Vector2(transform.position.x, objectifY - distanceMin * DirectionY),
				new Vector2(transform.position.x, objectifY - distanceMax * DirectionY),
				Color.green
			);
			Debug.DrawLine(
				new Vector2(transform.position.x, objectifY + distanceMin * DirectionY),
				new Vector2(transform.position.x, objectifY + distanceMax * DirectionY),
				Color.red
			);
			gizPos.x = objectifY;
		}

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(gizPos, 0.1f);
	}
#endif
}
