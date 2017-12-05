using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracks properties of basic UI screen such as number of instances
/// By: NeiLDG
/// </summary>
public class BasicUIScreenModel {
	private static BasicUIScreenModel sharedInstance = null;

	public static BasicUIScreenModel Instance {
		get {
			if (sharedInstance == null) {
				sharedInstance = new BasicUIScreenModel ();
			}

			return sharedInstance;
		}
	}

	private int numDuplicates = 1;

	private BasicUIScreenModel() {

	}

	public void IncrementDuplicate() {
		this.numDuplicates++;
	}

	public void DecrementDuplicate() {
		this.numDuplicates--;

		if (this.numDuplicates == 0) {
			sharedInstance = null;
		}
	}

	public int GetDuplicateCount() {
		return this.numDuplicates;
	}
}
