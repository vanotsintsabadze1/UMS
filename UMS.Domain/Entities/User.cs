﻿using UMS.Domain.Enums;
using UMS.Domain.ValueObjects;

namespace UMS.Domain.Entities;

public class User
{
    public required int Id { get; set; }
    public required string Firstname { get; set; }
    public required string Lastname { get; set; }
    public required Gender Gender { get; set; }
    public required string SocialNumber { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public required int CityId { get; set; }
    public City City { get; set; }
    public string? ImageUri { get; set; }
    public required ICollection<PhoneNumber> PhoneNumbers { get; set; }
    public ICollection<UserRelationship>? RelatedUsers { get; set; }
    public ICollection<UserRelationship>? RelatedByUsers { get; set; }
}