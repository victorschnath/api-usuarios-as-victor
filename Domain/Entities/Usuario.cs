namespace Domain.Entities;

public class Usuario
{
    public int Id { get; set; } // PK, Auto-increment
    public string Nome { get; set; } = string.Empty; // Obrigatório, 3-100 caracteres
    public string Email { get; set; } = string.Empty; // Obrigatório, formato válido, único
    public string Senha { get; set; } = string.Empty; // Obrigatório, min 6 caracteres
    public DateTime DataNascimento { get; set; } // Obrigatório, idade >= 18 anos
    public string? Telefone { get; set; } // Opcional, formato (XX) XXXXX-XXXX
    public bool Ativo { get; set; } = true; // Obrigatório, default true
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow; // Obrigatório, preenchido automaticamente
    public DateTime? DataAtualizacao { get; set; } // Opcional, atualizado automaticamente
}



