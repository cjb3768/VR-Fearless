using UnityEngine;
using System.Collections;

/**
 * Defines an interface for connected devices
 * C# implementation of ConnectedListener.java, in Bioharness SDK sample android project
 */

public interface ConnectedListener {
	public void Connected(ConnectedEvent<T> eventArgs);
}
