
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CommandStack
{
	private Stack<Command> cSt;
	public CommandStack ()
	{
		cSt = new Stack<Command> ();
	}

	public void Execute(Command c){
		Debug.Log ("Executing");
		cSt.Push (c);
	}
	public void Undo(){
		if(cSt.Count>0){
			Command c = cSt.Pop ();
			Debug.Log(c);
			c.Undo ();
		}
	}

	public void pushStack(CommandStack stack){
		Stack<Command> temp=new Stack<Command>();
		while (stack.cSt.Count>0) {
			temp.Push(stack.cSt.Pop());
		}
		while (temp.Count>0) {
			cSt.Push(temp.Pop());
		}
	}


}

