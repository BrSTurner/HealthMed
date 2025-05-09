using Med.Domain.Entites;

namespace Med.Infrastructure.Seeding
{
    public static class SpecialitySeeding
    {
        private static readonly DateTime CreatedAt = new (2025, 05, 07, 00, 00, 00);
        public static Speciality[] Specialities => 
        [
            new () { Id = Guid.Parse("4a4c0f8e-b28b-4e1b-a4ea-5a4b79fd9b6c"), Name = "Cardiologia", CreatedAt = CreatedAt },
            new () { Id = Guid.Parse("2c9d2f77-6c1e-4b47-83f0-f2d1ab3147f5"), Name = "Dermatologia", CreatedAt = CreatedAt },
            new () { Id = Guid.Parse("9d3b5c38-61c4-4bc6-b9ea-3acdb148c1ee"), Name = "Ortopedia", CreatedAt = CreatedAt },
            new () { Id = Guid.Parse("79c5e160-9de3-446a-b44c-9b6a982cc4f0"), Name = "Neurologia", CreatedAt = CreatedAt },
            new () { Id = Guid.Parse("87d7a295-0a4e-4c88-9367-0d83a746e6a7"), Name = "Endocrinologia", CreatedAt = CreatedAt },
            new () { Id = Guid.Parse("5811db18-b55b-4f34-95cf-11f3f0f89193"), Name = "Ginecologia e Obstetrícia", CreatedAt = CreatedAt },
            new () { Id = Guid.Parse("fde82618-bb0d-4a27-b232-3d3c204f00b4"), Name = "Psiquiatria", CreatedAt = CreatedAt },
            new () { Id = Guid.Parse("3f4a6e72-e22c-4ae0-ae7e-7201c6ddc9c2"), Name = "Pediatria", CreatedAt = CreatedAt },
            new () { Id = Guid.Parse("b2bc226c-96d3-401f-88e2-70e62f66ae8f"), Name = "Gastroenterologia", CreatedAt = CreatedAt },
            new () { Id = Guid.Parse("a8e05346-29e6-4f30-94e2-e4ecf77b0d4b"), Name = "Urologia", CreatedAt = CreatedAt },
        ];
    }
}
