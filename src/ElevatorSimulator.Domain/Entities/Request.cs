﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator.Domain.Entities;
public class Request
{
    public int CurrentFloor { get; set; }
    public int TargetFloor { get; set; }
    public int ObjectWaiting { get; set; }
}
