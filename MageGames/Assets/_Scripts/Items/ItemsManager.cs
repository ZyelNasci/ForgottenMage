using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : Singleton<MonoBehaviour>
{
	public ItemsSO so;

	private Dictionary<StaffType, StaffInfo> staffInfo = new Dictionary<StaffType, StaffInfo>();
	private Dictionary<RingType, RingInfo> ringInfo = new Dictionary<RingType, RingInfo>();

	private void Start()
	{
		//for (int i = 0; i < so.staffs.Length; i++)		
		//	staffInfo.Add(so.staffs[i].collectableType, so.staffs[i]);
		
		//for (int i = 0; i < so.rings.Length; i++)		
		//	ringInfo.Add(so.rings[i].type, so.rings[i]);		
	}

	public StaffInfo GetItemInfo(StaffType type)
	{
		return staffInfo[type];
	}

	public RingInfo GetItemInfo(RingType type)
	{
		return ringInfo[type];
	}
}