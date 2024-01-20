﻿namespace KantynaLaser.Web.Models;

public abstract class Entity
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; protected set; }
}
