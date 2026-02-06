using Domain.Entities;

namespace Application.Common.Exceptions;

public class RoleNotFoundException(int roleId)
    : BaseException(roleId, $"Role not found under id {roleId}");